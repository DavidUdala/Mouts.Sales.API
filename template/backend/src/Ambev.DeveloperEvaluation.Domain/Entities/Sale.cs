using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a sales transaction made at a specific branch by a customer.
    /// </summary>
    public class Sale : BaseEntity
    {
        /// <summary>
        /// Business-facing sale number used for tracking and reference.
        /// Must be unique across all sales.
        /// </summary>
        public string SaleNumber { get; set; } = string.Empty;
        /// <summary>
        /// Date and time when the sale was made.
        /// </summary>
        public DateTime SaleDate { get; set; }
        /// <summary>
        /// Foreign key referencing the customer who made the purchase.
        /// </summary>
        public Guid CustomerId { get; set; }
        /// <summary>
        /// Foreign key referencing the branch where the sale took place.
        /// </summary>
        public Guid BranchId { get; set; }
        /// <summary>
        /// Total monetary value of the sale, calculated as the sum of all item totals.
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// Indicates whether the sale has been cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }

        /// <summary>
        /// Navigation property to the customer associated with this sale.
        /// </summary>
        public User Customer { get; set; } = new();
        /// <summary>
        /// Navigation property to the branch where this sale was made.
        /// </summary>
        public Branch Branch { get; set; } = new();
        /// <summary>
        /// Collection of line items that compose this sale.
        /// Each item represents a product with its quantity, price, and discount.
        /// </summary>
        public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
    }
}