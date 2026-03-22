using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales
{
    /// <summary>
    /// Query parameters for retrieving a paginated and filtered list of sales.
    /// </summary>
    public class GetSalesRequest
    {
        /// <summary>Page number (1-based). Defaults to 1.</summary>
        [FromQuery(Name = "_page")]
        public int Page { get; set; } = 1;

        /// <summary>Number of items per page. Defaults to 10.</summary>
        [FromQuery(Name = "_size")]
        public int Size { get; set; } = 10;

        /// <summary>Filter by customer name. Supports wildcard: "John*" or "*Smith".</summary>
        /// <example>testuser</example>
        public string? CustomerName { get; set; }

        /// <summary>Filter by branch name. Supports wildcard: "Branch*" or "*Norte".</summary>
        /// <example>North</example>
        public string? BranchName { get; set; }

        /// <summary>Filter by exact sale number.</summary>
        /// <example>SALE-20260322-A1B2C3D4</example>
        public string? SaleNumber { get; set; }

        /// <summary>Filter by cancellation status.</summary>
        /// <example>false</example>
        public bool? IsCancelled { get; set; }

        /// <summary>Filter sales created on or after this date.</summary>
        /// <example>2026-01-01T00:00:00Z</example>
        public DateTime? MinDate { get; set; }

        /// <summary>Filter sales created on or before this date.</summary>
        /// <example>2026-12-31T23:59:59Z</example>
        public DateTime? MaxDate { get; set; }

        /// <summary>Filter sales with total amount >= this value.</summary>
        /// <example>0</example>
        public decimal? MinTotal { get; set; }

        /// <summary>Filter sales with total amount <= this value.</summary>
        /// <example>500</example>
        public decimal? MaxTotal { get; set; }

        /// <summary>Ordering string, e.g. "saleDate desc, totalAmount asc".</summary>
        /// <example>saleDate desc</example>
        [FromQuery(Name = "_order")]
        public string? Order { get; set; }
    }
}