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
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the branch where the sale is taking place.
        /// </summary>
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
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the number of units of the product. Maximum of 20 identical items per sale.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the price per unit of the product at the time of the sale.
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}