using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Query for retrieving a paginated and filtered list of sales.
/// </summary>
public class GetSalesQuery : IRequest<GetSalesResult>
{
    /// <summary>Page number (1-based).</summary>
    public int Page { get; set; } = 1;

    /// <summary>Number of items per page.</summary>
    public int Size { get; set; } = 10;

    /// <summary>Filters by customer name. Supports wildcard: "John*" or "*Smith".</summary>
    public string? CustomerName { get; set; }

    /// <summary>Filters by branch name. Supports wildcard: "Branch*" or "*Norte".</summary>
    public string? BranchName { get; set; }

    /// <summary>Filters by exact sale number.</summary>
    public string? SaleNumber { get; set; }

    /// <summary>Filters by cancellation status.</summary>
    public bool? IsCancelled { get; set; }

    /// <summary>Filters sales created on or after this date.</summary>
    public DateTime? MinDate { get; set; }

    /// <summary>Filters sales created on or before this date.</summary>
    public DateTime? MaxDate { get; set; }

    /// <summary>Filters sales with total amount greater than or equal to this value.</summary>
    public decimal? MinTotal { get; set; }

    /// <summary>Filters sales with total amount less than or equal to this value.</summary>
    public decimal? MaxTotal { get; set; }
}
