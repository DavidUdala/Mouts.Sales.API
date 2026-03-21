using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;
using Rebus.Bus;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Handler for processing UpdateSaleCommand requests
/// </summary>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IBus _bus;
    private readonly ILogger<UpdateSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of UpdateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="bus">The Rebus bus instance</param>
    /// <param name="logger">The logger instance</param>
    public UpdateSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        IBus bus,
        ILogger<UpdateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _bus = bus;
        _logger = logger;
    }

    /// <summary>
    /// Handles the UpdateSaleCommand request
    /// </summary>
    /// <param name="command">The UpdateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale details</returns>
    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingSale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (existingSale == null)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        // Detect items that were removed (cancelled) from the sale
        var previousProductIds = existingSale.Items.Select(i => i.ProductId).ToHashSet();

        _mapper.Map(command, existingSale);

        var currentProductIds = existingSale.Items.Select(i => i.ProductId).ToHashSet();
        var cancelledProductIds = previousProductIds.Except(currentProductIds).ToList();

        existingSale.ApplyDiscounts();
        existingSale.CalculateTotals();

        var updatedSale = await _saleRepository.UpdateAsync(existingSale, cancellationToken);

        _logger.LogInformation(
            "Publishing {EventName} to message broker for Sale {SaleNumber}",
            nameof(SaleModifiedEvent),
            updatedSale.SaleNumber);

        await _bus.Publish(new SaleModifiedEvent(updatedSale.Id, updatedSale.SaleNumber, updatedSale.TotalAmount));

        // Publish ItemCancelledEvent for each removed item
        foreach (var productId in cancelledProductIds)
        {
            _logger.LogInformation(
                "Publishing {EventName} to message broker for ProductId {ProductId} from Sale {SaleNumber}",
                nameof(ItemCancelledEvent),
                productId,
                updatedSale.SaleNumber);

            await _bus.Publish(new ItemCancelledEvent(updatedSale.Id, updatedSale.SaleNumber, productId));
        }

        var result = _mapper.Map<UpdateSaleResult>(updatedSale);
        return result;
    }
}
