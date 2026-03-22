namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Represents the response returned after successfully updating a sale.
/// </summary>
public class UpdateSaleResult
{
    /// <summary>
    /// The unique identifier of the updated sale
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The business-facing sale number of the updated sale
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;
}