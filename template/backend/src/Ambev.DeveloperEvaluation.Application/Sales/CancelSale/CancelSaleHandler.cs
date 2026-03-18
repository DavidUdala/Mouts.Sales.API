using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Rebus.Bus;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Handler for processing CancelSaleCommand requests
/// </summary>
public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResponse>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IBus _bus;
    private readonly ILogger<CancelSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of CancelSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="bus">The Rebus bus instance</param>
    /// <param name="logger">The logger instance</param>
    public CancelSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        IBus bus,
        ILogger<CancelSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _bus = bus;
        _logger = logger;
    }

    /// <summary>
    /// Handles the CancelSaleCommand request
    /// </summary>
    /// <param name="command">The CancelSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the cancel operation</returns>
    public async Task<CancelSaleResponse> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CancelSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingSale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (existingSale == null)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        if (existingSale.IsCancelled)
            throw new InvalidOperationException($"Sale with ID {command.Id} has already been cancelled");

        existingSale.IsCancelled = true;

        var updatedSale = await _saleRepository.UpdateAsync(existingSale, cancellationToken);

        _logger.LogInformation(
            "Publishing {EventName} to message broker for Sale {SaleNumber}",
            nameof(SaleCancelledEvent),
            updatedSale.SaleNumber);

        await _bus.Publish(new SaleCancelledEvent(updatedSale.Id, updatedSale.SaleNumber));

        var result = _mapper.Map<CancelSaleResponse>(updatedSale);
        return result;
    }
}
