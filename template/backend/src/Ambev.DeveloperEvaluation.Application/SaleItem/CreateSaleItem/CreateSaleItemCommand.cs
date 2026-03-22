namespace Ambev.DeveloperEvaluation.Application.SaleItem.CreateSaleItem
{

    /// <summary>
    /// Represents a line item in the create sale command.
    /// </summary>
    public class CreateSaleItemCommand
    {
        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price of the product at the time of sale.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the discount applied to this item.
        /// </summary>
        public decimal Discount { get; set; }
    }
}
