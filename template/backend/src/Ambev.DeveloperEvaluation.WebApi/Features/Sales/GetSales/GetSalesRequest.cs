namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales
{
    /// <summary>
    /// Query parameters for retrieving a paginated and filtered list of sales.
    /// </summary>
    public class GetSalesRequest
    {
        /// <summary>Page number (1-based). Defaults to 1.</summary>
        public int Page { get; set; } = 1;

        /// <summary>Number of items per page. Defaults to 10.</summary>
        public int Size { get; set; } = 10;

        /// <summary>Filter by customer name. Supports wildcard: "John*" or "*Smith".</summary>
        public string? CustomerName { get; set; }

        /// <summary>Filter by branch name. Supports wildcard: "Branch*" or "*Norte".</summary>
        public string? BranchName { get; set; }

        /// <summary>Filter by exact sale number.</summary>
        public string? SaleNumber { get; set; }

        /// <summary>Filter by cancellation status.</summary>
        public bool? IsCancelled { get; set; }

        /// <summary>Filter sales created on or after this date.</summary>
        public DateTime? MinDate { get; set; }

        /// <summary>Filter sales created on or before this date.</summary>
        public DateTime? MaxDate { get; set; }

        /// <summary>Filter sales with total amount >= this value.</summary>
        public decimal? MinTotal { get; set; }

        /// <summary>Filter sales with total amount <= this value.</summary>
        public decimal? MaxTotal { get; set; }
    }
}