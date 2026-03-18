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
    public string SaleNumber { get; }

    /// <summary>
    /// The unique identifier of the product that was cancelled.
    /// </summary>
    public Guid ProductId { get; }

    /// <summary>
    /// The date when the event occurred.
    /// </summary>
    public DateTime OccurredAt { get; }

    public ItemCancelledEvent(Guid saleId, string saleNumber, Guid productId)
    {
        SaleId = saleId;
        SaleNumber = saleNumber;
        ProductId = productId;
        OccurredAt = DateTime.UtcNow;
    }
}
