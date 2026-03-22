using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events.Handlers;

/// <summary>
/// Rebus handler that processes ItemCancelledEvent messages and logs the occurrence.
/// </summary>
public class ItemCancelledEventHandler : IHandleMessages<ItemCancelledEvent>
{
    private readonly ILogger<ItemCancelledEventHandler> _logger;

    public ItemCancelledEventHandler(ILogger<ItemCancelledEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ItemCancelledEvent message)
    {
        _logger.LogInformation(
            "Event {EventName} - Item (SaleItemId: {SaleItemId}) cancelled from Sale {SaleNumber} (ID: {SaleId}) at {OccurredAt}",
            nameof(ItemCancelledEvent),
            message.SaleItemId,
            message.SaleNumber,
            message.SaleId,
            message.OccurredAt);

        return Task.CompletedTask;
    }
}
