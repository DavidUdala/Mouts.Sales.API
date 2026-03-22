using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Rebus.Bus;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

/// <summary>
/// Handler for cancelling a specific item within a sale.
/// </summary>
public class CancelSaleItemHandler : IRequestHandler<CancelSaleItemCommand, CancelSaleItemResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IBus _bus;
    private readonly ILogger<CancelSaleItemHandler> _logger;

    public CancelSaleItemHandler(
        ISaleRepository saleRepository,
        IBus bus,
        ILogger<CancelSaleItemHandler> logger)
    {
        _saleRepository = saleRepository;
        _bus = bus;
        _logger = logger;
    }

    public async Task<CancelSaleItemResult> Handle(CancelSaleItemCommand command, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken)
            ?? throw new KeyNotFoundException($"Sale with ID {command.SaleId} not found.");

        if (sale.IsCancelled)
            throw new InvalidOperationException($"Sale '{sale.SaleNumber}' is cancelled and cannot be modified.");

        sale.CancelItem(command.ItemId);

        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        _logger.LogInformation(
            "Publishing {EventName} to message broker for Item {ItemId} from Sale {SaleNumber}",
            nameof(ItemCancelledEvent),
            command.ItemId,
            updatedSale.SaleNumber);

        await _bus.Publish(new ItemCancelledEvent(updatedSale.Id, command.ItemId, updatedSale.SaleNumber));

        return new CancelSaleItemResult
        {
            SaleId = updatedSale.Id,
            SaleNumber = updatedSale.SaleNumber,
            ItemId = command.ItemId,
            IsCancelled = true
        };
    }
}
