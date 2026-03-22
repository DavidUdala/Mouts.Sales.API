namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event published when a sale is cancelled.
/// </summary>
public class SaleCancelledEvent
{
    /// <summary>
    /// The unique identifier of the sale.
    /// </summary>
    public Guid SaleId { get; }

    /// <summary>
    /// The business-facing sale number.
    /// </summary>
    public string SaleNumber { get; }

    /// <summary>
    /// The date when the event occurred.
    /// </summary>
    public DateTime OccurredAt { get; }

    public SaleCancelledEvent(Guid saleId, string saleNumber)
    {
        SaleId = saleId;
        SaleNumber = saleNumber;
        OccurredAt = DateTime.UtcNow;
    }
}
