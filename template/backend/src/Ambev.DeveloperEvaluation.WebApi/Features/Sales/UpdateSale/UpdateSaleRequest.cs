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
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the price per unit of the product at the time of the sale.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the number of units of the product. Maximum of 20 identical items per sale.
        /// </summary>
        public int Quantity { get; set; }
    }
}
