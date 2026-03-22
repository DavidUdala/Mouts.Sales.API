using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events.Handlers;

/// <summary>
/// Rebus handler that processes SaleModifiedEvent messages and logs the occurrence.
/// </summary>
public class SaleModifiedEventHandler : IHandleMessages<SaleModifiedEvent>
{
    private readonly ILogger<SaleModifiedEventHandler> _logger;

    public SaleModifiedEventHandler(ILogger<SaleModifiedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleModifiedEvent message)
    {
        _logger.LogInformation(
            "Event {EventName} - Sale {SaleNumber} (ID: {SaleId}) modified with new total amount {TotalAmount:C} at {OccurredAt}",
            nameof(SaleModifiedEvent),
            message.SaleNumber,
            message.SaleId,
            message.TotalAmount,
            message.OccurredAt);

        return Task.CompletedTask;
    }
}
