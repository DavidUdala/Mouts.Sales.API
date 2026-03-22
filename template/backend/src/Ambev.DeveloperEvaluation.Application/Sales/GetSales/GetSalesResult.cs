namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Result returned by the GetSales query, including pagination metadata.
/// </summary>
public class GetSalesResult
{
    /// <summary>The page of sale records matching the query.</summary>
    public IEnumerable<GetSalesItemResult> Items { get; set; } = [];

    /// <summary>Total number of records matching the filters (before pagination).</summary>
    public int TotalCount { get; set; }

    /// <summary>Current page number.</summary>
    public int Page { get; set; }

    /// <summary>Number of items per page.</summary>
    public int PageSize { get; set; }
}

/// <summary>
/// Represents a single sale entry in the paginated list result.
/// </summary>
public class GetSalesItemResult
{
    /// <summary>Unique identifier of the sale.</summary>
    public Guid Id { get; set; }

    /// <summary>Business-facing sale number.</summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>Date and time when the sale was created.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Date and time of the last update to the sale.</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Unique identifier of the customer.</summary>
    public Guid CustomerId { get; set; }

    /// <summary>Denormalized name of the customer at the time of the sale.</summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>Unique identifier of the branch.</summary>
    public Guid BranchId { get; set; }

    /// <summary>Denormalized name of the branch at the time of the sale.</summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>Total monetary value of the sale after discounts.</summary>
    public decimal TotalAmount { get; set; }

    /// <summary>Indicates whether the sale has been cancelled.</summary>
    public bool IsCancelled { get; set; }

    /// <summary>Line items composing the sale.</summary>
    public List<GetSalesLineItemResult> Items { get; set; } = [];
}

/// <summary>
/// Represents a single line item within a sale entry.
/// </summary>
public class GetSalesLineItemResult
{
    /// <summary>Unique identifier of the sale item.</summary>
    public Guid Id { get; set; }

    /// <summary>Unique identifier of the product.</summary>
    public Guid ProductId { get; set; }

    /// <summary>Denormalized name of the product at the time of the sale.</summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>Number of units sold.</summary>
    public int Quantity { get; set; }

    /// <summary>Price per unit at the time of the sale.</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>Discount percentage applied to this line item.</summary>
    public decimal Discount { get; set; }

    /// <summary>Total monetary value of this line item after discount.</summary>
    public decimal TotalAmount { get; set; }
}
