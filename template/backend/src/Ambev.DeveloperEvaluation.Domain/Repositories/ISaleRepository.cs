using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Sale entity operations
/// </summary>
public interface ISaleRepository
{
    /// <summary>
    /// Creates a new sale in the repository
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a sale by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a sale by its sale number
    /// </summary>
    /// <param name="saleNumber">The sale number to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing sale in the repository
    /// </summary>
    /// <param name="sale">The sale to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale</returns>
    Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a sale from the repository
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale was deleted, false if not found</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated and filtered list of sales.
    /// </summary>
    /// <param name="page">Page number (1-based)</param>
    /// <param name="size">Number of items per page</param>
    /// <param name="customerName">Filter by customer name — supports wildcard prefix/suffix (*)</param>
    /// <param name="branchName">Filter by branch name — supports wildcard prefix/suffix (*)</param>
    /// <param name="saleNumber">Filter by exact sale number</param>
    /// <param name="isCancelled">Filter by cancellation status</param>
    /// <param name="minDate">Filter sales created on or after this date</param>
    /// <param name="maxDate">Filter sales created on or before this date</param>
    /// <param name="minTotal">Filter sales with total amount >= this value</param>
    /// <param name="maxTotal">Filter sales with total amount <= this value</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A tuple with the matching sales and the total count before pagination</returns>
    Task<(IEnumerable<Sale> Items, int TotalCount)> GetSalesAsync(
        int page,
        int size,
        string? customerName = null,
        string? branchName = null,
        string? saleNumber = null,
        bool? isCancelled = null,
        DateTime? minDate = null,
        DateTime? maxDate = null,
        decimal? minTotal = null,
        decimal? maxTotal = null,
        CancellationToken cancellationToken = default);
}
