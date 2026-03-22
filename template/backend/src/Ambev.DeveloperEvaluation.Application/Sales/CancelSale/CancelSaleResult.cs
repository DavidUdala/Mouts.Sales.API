namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Response model for CancelSale operation
/// </summary>
public class CancelSaleResult
{
    /// <summary>
    /// The unique identifier of the cancelled sale.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The business-facing number of the cancelled sale.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the sale is cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }
}
