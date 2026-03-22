namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event published when an existing sale is modified.
/// </summary>
public class SaleModifiedEvent
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
    /// The new total amount after modification.
    /// </summary>
    public decimal TotalAmount { get; }

    /// <summary>
    /// The date when the event occurred.
    /// </summary>
    public DateTime OccurredAt { get; }

    public SaleModifiedEvent(Guid saleId, string saleNumber, decimal totalAmount)
    {
        SaleId = saleId;
        SaleNumber = saleNumber;
        TotalAmount = totalAmount;
        OccurredAt = DateTime.UtcNow;
    }
}
