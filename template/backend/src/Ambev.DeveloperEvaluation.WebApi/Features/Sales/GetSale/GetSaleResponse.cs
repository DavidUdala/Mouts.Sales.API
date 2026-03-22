namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    /// <summary>
    /// Response model returned when retrieving a sale by ID
    /// </summary>
    public class GetSaleResponse
    {
        /// <summary>The unique identifier of the sale</summary>
        public Guid Id { get; set; }

        /// <summary>The business-facing sale number (e.g. SALE-20260322-A1B2C3D4)</summary>
        public string SaleNumber { get; set; } = string.Empty;

        /// <summary>The date and time when the sale was created</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>The date and time when the sale was last updated</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>The unique identifier of the customer</summary>
        public Guid CustomerId { get; set; }

        /// <summary>The denormalized customer name at the time of the sale</summary>
        public string CustomerName { get; set; } = string.Empty;

        /// <summary>The unique identifier of the branch</summary>
        public Guid BranchId { get; set; }

        /// <summary>The denormalized branch name at the time of the sale</summary>
        public string BranchName { get; set; } = string.Empty;

        /// <summary>The total monetary value of the sale after discounts</summary>
        public decimal TotalAmount { get; set; }

        /// <summary>Indicates whether the sale has been cancelled</summary>
        public bool IsCancelled { get; set; }

        /// <summary>The list of items included in the sale</summary>
        public List<GetSaleItemResponse> Items { get; set; } = [];
    }

    /// <summary>
    /// Response model for a single line item within a sale
    /// </summary>
    public class GetSaleItemResponse
    {
        /// <summary>The unique identifier of the sale item</summary>
        public Guid Id { get; set; }

        /// <summary>The unique identifier of the product</summary>
        public Guid ProductId { get; set; }

        /// <summary>The denormalized product name at the time of the sale</summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>The quantity of the product sold</summary>
        public int Quantity { get; set; }

        /// <summary>The price per unit at the time of the sale</summary>
        public decimal UnitPrice { get; set; }

        /// <summary>The discount rate applied to this item (e.g. 0.10 for 10%)</summary>
        public decimal Discount { get; set; }

        /// <summary>The total amount for this item after discount</summary>
        public decimal TotalAmount { get; set; }

        /// <summary>Indicates whether this item has been individually cancelled</summary>
        public bool IsCancelled { get; set; }
    }
}