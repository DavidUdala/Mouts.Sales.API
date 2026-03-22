namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    /// <summary>
    /// Represents a request to Update a sale in the system.
    /// </summary>
    public class UpdateSaleRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sale.
        /// </summary>
        /// <example>00000000-0000-0000-0000-000000000000</example>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the list of items to be included in the sale.
        /// </summary>
        public UpdateSaleItemRequest Item { get; set; } = new();
    }

    public class UpdateSaleItemRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the item sale.
        /// </summary>
        /// <example>00000000-0000-0000-0000-000000000000</example>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the price per unit of the product at the time of the sale.
        /// </summary>
        /// <example>9.90</example>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the number of units of the product. Maximum of 20 identical items per sale.
        /// </summary>
        /// <example>5</example>
        public int Quantity { get; set; }
    }
}
