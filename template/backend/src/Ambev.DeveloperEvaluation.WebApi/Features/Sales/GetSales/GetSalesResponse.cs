namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales
{
    public class GetSalesResponse
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public Guid BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }
        public List<GetSalesItemResponse> Items { get; set; } = [];
    }
    public class GetSalesItemResponse
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }
    }
}