using Ambev.DeveloperEvaluation.Application.SaleItem.GetSaleItem;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Response model for GetSale operation
/// </summary>
public class GetSaleResult
{
    /// <summary>
    /// The business-facing sale number
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// The date and time when the sale was made
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// The customer identifier
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// The branch identifier
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// The total monetary value of the sale
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Indicates whether the sale has been cancelled
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// The line items of the sale
    /// </summary>
    public List<GetSaleItemResult> Items { get; set; } = [];
}