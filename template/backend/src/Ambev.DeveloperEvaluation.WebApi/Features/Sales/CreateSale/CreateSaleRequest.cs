namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    /// <summary>
    /// Represents a request to create a new sale in the system.
    /// </summary>
    public class CreateSaleRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the customer making the purchase.
        /// </summary>
        /// <example>00000000-0000-0000-0000-000000000000</example>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the branch where the sale is taking place.
        /// </summary>
        /// <example>11111111-1111-1111-1111-111111111111</example>
        public Guid BranchId { get; set; }

        /// <summary>
        /// Gets or sets the list of items to be included in the sale.
        /// </summary>
        public List<CreateSaleItemRequest> Items { get; set; } = [];
    }

    /// <summary>
    /// Represents a request to create a new sale item in the sale.
    /// </summary>
    public class CreateSaleItemRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product being sold.
        /// </summary>
        /// <example>aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa</example>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the number of units of the product. Maximum of 20 identical items per sale.
        /// </summary>
        /// <example>5</example>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the price per unit of the product at the time of the sale.
        /// </summary>
        /// <example>8.90</example>
        public decimal UnitPrice { get; set; }
    }
}