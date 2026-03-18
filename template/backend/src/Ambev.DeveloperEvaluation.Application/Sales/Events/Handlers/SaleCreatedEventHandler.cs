using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events.Handlers;

/// <summary>
/// Rebus handler that processes SaleCreatedEvent messages and logs the occurrence.
/// </summary>
public class SaleCreatedEventHandler : IHandleMessages<SaleCreatedEvent>
{
    private readonly ILogger<SaleCreatedEventHandler> _logger;

    public SaleCreatedEventHandler(ILogger<SaleCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleCreatedEvent message)
    {
        _logger.LogInformation(
            "Event {EventName} - Sale {SaleNumber} (ID: {SaleId}) created with total amount {TotalAmount:C} at {OccurredAt}",
            nameof(SaleCreatedEvent),
            message.SaleNumber,
            message.SaleId,
            message.TotalAmount,
            message.OccurredAt);

        return Task.CompletedTask;
    }
}
