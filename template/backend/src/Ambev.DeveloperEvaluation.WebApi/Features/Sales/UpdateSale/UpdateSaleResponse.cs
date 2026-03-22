namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    /// <summary>
    /// Response model returned after successfully updating a sale item
    /// </summary>
    public class UpdateSaleResponse
    {
        /// <summary>The unique identifier of the updated sale</summary>
        public Guid Id { get; set; }

        /// <summary>The business-facing sale number of the updated sale</summary>
        public string SaleNumber { get; set; } = string.Empty;
    }
}