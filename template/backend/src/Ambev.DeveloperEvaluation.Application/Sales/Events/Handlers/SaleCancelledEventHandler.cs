using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events.Handlers;

/// <summary>
/// Rebus handler that processes SaleCancelledEvent messages and logs the occurrence.
/// </summary>
public class SaleCancelledEventHandler : IHandleMessages<SaleCancelledEvent>
{
    private readonly ILogger<SaleCancelledEventHandler> _logger;

    public SaleCancelledEventHandler(ILogger<SaleCancelledEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleCancelledEvent message)
    {
        _logger.LogInformation(
            "Event {EventName} - Sale {SaleNumber} (ID: {SaleId}) was cancelled at {OccurredAt}",
            nameof(SaleCancelledEvent),
            message.SaleNumber,
            message.SaleId,
            message.OccurredAt);

        return Task.CompletedTask;
    }
}
