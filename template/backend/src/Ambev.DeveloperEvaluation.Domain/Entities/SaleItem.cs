using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a single line item within a sale, linking a product to its
    /// quantity, pricing, and discount information.
    /// </summary>
    public class SaleItem : BaseEntity
    {
        /// <summary>
        /// Foreign key referencing the parent sale this item belongs to.
        /// </summary>
        public Guid SaleId { get; set; }

        /// <summary>
        /// Foreign key referencing the product being sold in this line item.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Number of units of the product sold in this line item.
        /// Must be greater than zero.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Price per unit of the product at the time of the sale.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Discount amount applied to this line item.
        /// Defaults to zero when no discount is applied.
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Total monetary value for this line item after applying the discount.
        /// Calculated as: (Quantity * UnitPrice) - ((Quantity * UnitPrice) * Discount / 100).
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Navigation property to the parent sale.
        /// </summary>
        public Sale Sale { get; set; } = new ();

        /// <summary>
        /// Navigation property to the product associated with this line item.
        /// </summary>
        public Product Product { get; set; } = new Product(); 
    }
}
