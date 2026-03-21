namespace Ambev.DeveloperEvaluation.Application.SaleItem.GetSaleItem
{
    /// <summary>
    /// Response model for a sale line item
    /// </summary>
    public class GetSaleItemResult
    {
        public Guid Id { get; set; }

        /// <summary>
        /// The product identifier
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// The quantity of the product
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The unit price of the product
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// The discount applied to this item
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// The total amount for this line item
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Indicates whether the sale item has been cancelled
        /// </summary>
        public bool IsCancelled { get; set; }
    }
}