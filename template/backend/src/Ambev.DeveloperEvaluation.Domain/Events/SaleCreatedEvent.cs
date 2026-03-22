namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event published when a new sale is created.
/// </summary>
public class SaleCreatedEvent
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
    /// The total amount of the sale.
    /// </summary>
    public decimal TotalAmount { get; }

    /// <summary>
    /// The date when the event occurred.
    /// </summary>
    public DateTime OccurredAt { get; }

    public SaleCreatedEvent(Guid saleId, string saleNumber, decimal totalAmount)
    {
        SaleId = saleId;
        SaleNumber = saleNumber;
        TotalAmount = totalAmount;
        OccurredAt = DateTime.UtcNow;
    }
}
