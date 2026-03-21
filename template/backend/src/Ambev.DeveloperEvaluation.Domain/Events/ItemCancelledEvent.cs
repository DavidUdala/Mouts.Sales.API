namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event published when a specific item within a sale is cancelled.
/// </summary>
public class ItemCancelledEvent
{
    /// <summary>
    /// The unique identifier of the sale.
    /// </summary>
    public Guid SaleId { get; }

    /// <summary>
    /// The business-facing sale number.
    /// </summary>
    public Guid SaleItemId { get; }

    public string SaleNumber { get; set; }

    /// <summary>
    /// The date when the event occurred.
    /// </summary>
    public DateTime OccurredAt { get; }

    public ItemCancelledEvent(Guid saleId, Guid saleItemId, string saleNumber)
    {
        SaleId = saleId;
        SaleItemId = saleItemId;
        OccurredAt = DateTime.UtcNow;
        SaleNumber = saleNumber;
    }
}
