namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;

/// <summary>
/// Response model returned after cancelling a specific item within a sale
/// </summary>
public class CancelSaleItemResponse
{
    /// <summary>The unique identifier of the sale that contains the cancelled item</summary>
    public Guid SaleId { get; set; }

    /// <summary>The business-facing sale number</summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>The unique identifier of the cancelled item</summary>
    public Guid ItemId { get; set; }

    /// <summary>Indicates whether the item was successfully cancelled (always true on success)</summary>
    public bool IsCancelled { get; set; }
}
