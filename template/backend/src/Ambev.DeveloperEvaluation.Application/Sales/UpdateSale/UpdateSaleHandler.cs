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
        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        if (sale.IsCancelled)
            throw new InvalidOperationException($"Sale '{sale.SaleNumber}' is cancelled and cannot be modified.");

        sale.UpdateItem(command.Item.Id, command.Item.Quantity, command.Item.UnitPrice);

        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        _logger.LogInformation(
            "Publishing {EventName} to message broker for Sale {SaleNumber}",
            nameof(SaleModifiedEvent),
            updatedSale.SaleNumber);

        await _bus.Publish(new SaleModifiedEvent(updatedSale.Id, updatedSale.SaleNumber, updatedSale.TotalAmount));

        var result = _mapper.Map<UpdateSaleResult>(updatedSale);
        return result;
    }
}
