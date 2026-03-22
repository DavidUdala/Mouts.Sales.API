namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

/// <summary>
/// Result model for CancelSaleItem operation.
/// </summary>
public class CancelSaleItemResult
{
    /// <summary>
    /// The unique identifier of the sale.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// The business-facing number of the sale.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// The unique identifier of the cancelled item.
    /// </summary>
    public Guid ItemId { get; set; }

    /// <summary>
    /// Indicates whether the item is cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }
}