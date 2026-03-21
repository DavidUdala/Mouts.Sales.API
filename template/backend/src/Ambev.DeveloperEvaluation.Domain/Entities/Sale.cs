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
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets the date and time of the last update to the sale's information.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

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


        public Sale()
        {
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Generates a unique, business-readable sale number based on the sale date and entity identity.
        /// Format: SALE-{YYYYMMDD}-{8-char hex from ID}
        /// Example: SALE-20260320-A3F2B1C4
        /// </summary>
        public void GenerateSaleNumber()
        {
            var datePart = CreatedAt.ToString("yyyyMMdd");
            var uniquePart = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
            SaleNumber = $"SALE-{datePart}-{uniquePart}";
        }

        /// <summary>
        /// Applies quantity-based discount tiers to each item in the sale and validates
        /// business constraints before the totals are calculated.
        /// Quantities are aggregated per product across all line items before validation,
        /// preventing the same product from being split across multiple lines to bypass limits.
        /// Rules:
        ///   - Above 20 identical items:  not allowed (throws DomainException)
        ///   - 10 to 20 identical items:  20% discount
        ///   - 4 to 9 identical items:    10% discount
        ///   - Below 4 identical items:    0% discount (no discount permitted)
        /// </summary>
        /// <exception cref="DomainException">
        /// Thrown when the total quantity of any product across all line items exceeds 20.
        /// </exception>
        public void ApplyDiscounts()
        {
            var totalQuantityByProduct = Items
                .GroupBy(i => i.ProductId)
                .ToDictionary(g => g.Key, g => g.Sum(i => i.Quantity));

            foreach (var item in Items)
            {
                var totalQuantity = totalQuantityByProduct[item.ProductId];

                if (totalQuantity > 20)
                    throw new DomainException(
                        $"Cannot sell more than 20 identical items. Product '{item.ProductId}' has {totalQuantity} units across all lines.");

                item.Discount = totalQuantity switch
                {
                    >= 10 => 20m,
                    >= 4  => 10m,
                    _     => 0m
                };
            }
        }

        /// <summary>
        /// Recalculates the total amount for each item and the overall sale total.
        /// Must be called after <see cref="ApplyDiscounts"/> to ensure discounts are reflected.
        /// </summary>
        public void CalculateTotals()
        {
            foreach (var item in Items)
            {
                item.TotalAmount = (item.Quantity * item.UnitPrice) * (1 - item.Discount / 100);
            }

            TotalAmount = Items.Sum(i => i.TotalAmount);
        }

        /// <summary>
        /// Cancels the sale and all its line items.
        /// </summary>
        /// <exception cref="DomainException">Thrown when the sale is already cancelled.</exception>
        public void Cancel()
        {
            if (IsCancelled)
                throw new DomainException($"Sale '{SaleNumber}' is already cancelled.");

            IsCancelled = true;

            foreach (var item in Items.Where(i => !i.IsCancelled))
                item.Cancel();

            Update();
        }

        /// <summary>
        /// Marks the sale as updated by refreshing the UpdatedAt timestamp.
        /// </summary>
        public void Update()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}