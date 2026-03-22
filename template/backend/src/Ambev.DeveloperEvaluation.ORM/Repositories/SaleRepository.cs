using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ISaleRepository using Entity Framework Core
/// </summary>
public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of SaleRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new sale in the database
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        // Prevent EF Core from traversing = new() stubs and inserting ghost entities.
        // The FK properties (CustomerId, BranchId, ProductId) carry the real references.
        sale.Customer = null!;
        sale.Branch = null!;
        foreach (var item in sale.Items)
        {
            item.Product = null!;
            item.Sale = null!;
        }

        await _context.Sales.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    /// <summary>
    /// Retrieves a sale by its unique identifier, including related items
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Branch)
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a sale by its sale number
    /// </summary>
    /// <param name="saleNumber">The sale number to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.SaleNumber == saleNumber, cancellationToken);
    }

    /// <summary>
    /// Updates an existing sale in the database
    /// </summary>
    /// <param name="sale">The sale to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale</returns>
    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    /// <summary>
    /// Retrieves a paginated and filtered list of sales
    /// </summary>
    public async Task<(IEnumerable<Sale> Items, int TotalCount)> GetSalesAsync(
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
        string? order = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Sales
            .AsNoTracking()
            .Include(s => s.Customer)
            .Include(s => s.Branch)
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(customerName))
            query = customerName switch
            {
                var v when v.StartsWith("*") => query.Where(s => EF.Functions.ILike(s.Customer.Username, $"%{v.TrimStart('*')}")),
                var v when v.EndsWith("*")   => query.Where(s => EF.Functions.ILike(s.Customer.Username, $"{v.TrimEnd('*')}%")),
                _                            => query.Where(s => EF.Functions.ILike(s.Customer.Username, $"%{customerName}%"))
            };

        if (!string.IsNullOrWhiteSpace(branchName))
            query = branchName switch
            {
                var v when v.StartsWith("*") => query.Where(s => EF.Functions.ILike(s.Branch.Name, $"%{v.TrimStart('*')}")),
                var v when v.EndsWith("*")   => query.Where(s => EF.Functions.ILike(s.Branch.Name, $"{v.TrimEnd('*')}%")),
                _                            => query.Where(s => EF.Functions.ILike(s.Branch.Name, $"%{branchName}%"))
            };

        if (!string.IsNullOrWhiteSpace(saleNumber))
            query = query.Where(s => s.SaleNumber == saleNumber);

        if (isCancelled.HasValue)
            query = query.Where(s => s.IsCancelled == isCancelled.Value);

        if (minDate.HasValue)
            query = query.Where(s => s.CreatedAt >= minDate.Value);

        if (maxDate.HasValue)
            query = query.Where(s => s.CreatedAt <= maxDate.Value);

        if (minTotal.HasValue)
            query = query.Where(s => s.TotalAmount >= minTotal.Value);

        if (maxTotal.HasValue)
            query = query.Where(s => s.TotalAmount <= maxTotal.Value);

        query = ApplyOrdering(query, order);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    private static IQueryable<Sale> ApplyOrdering(IQueryable<Sale> query, string? order)
    {
        if (string.IsNullOrWhiteSpace(order))
            return query.OrderByDescending(s => s.CreatedAt);

        IOrderedQueryable<Sale>? ordered = null;
        foreach (var segment in order.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var parts = segment.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var field = parts[0].ToLowerInvariant();
            var desc = parts.Length > 1 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

            ordered = (ordered, field, desc) switch
            {
                (null, "salenumber",  false) => query.OrderBy(s => s.SaleNumber),
                (null, "salenumber",  true)  => query.OrderByDescending(s => s.SaleNumber),
                (null, "saledate" or "createdat", false) => query.OrderBy(s => s.CreatedAt),
                (null, "saledate" or "createdat", true)  => query.OrderByDescending(s => s.CreatedAt),
                (null, "totalamount", false) => query.OrderBy(s => s.TotalAmount),
                (null, "totalamount", true)  => query.OrderByDescending(s => s.TotalAmount),
                (null, "customername", false) => query.OrderBy(s => s.Customer.Username),
                (null, "customername", true)  => query.OrderByDescending(s => s.Customer.Username),
                (null, "branchname",  false) => query.OrderBy(s => s.Branch.Name),
                (null, "branchname",  true)  => query.OrderByDescending(s => s.Branch.Name),
                (null, _, _) => query.OrderByDescending(s => s.CreatedAt),

                (_, "salenumber",  false) => ordered!.ThenBy(s => s.SaleNumber),
                (_, "salenumber",  true)  => ordered!.ThenByDescending(s => s.SaleNumber),
                (_, "saledate" or "createdat", false) => ordered!.ThenBy(s => s.CreatedAt),
                (_, "saledate" or "createdat", true)  => ordered!.ThenByDescending(s => s.CreatedAt),
                (_, "totalamount", false) => ordered!.ThenBy(s => s.TotalAmount),
                (_, "totalamount", true)  => ordered!.ThenByDescending(s => s.TotalAmount),
                (_, "customername", false) => ordered!.ThenBy(s => s.Customer.Username),
                (_, "customername", true)  => ordered!.ThenByDescending(s => s.Customer.Username),
                (_, "branchname",  false) => ordered!.ThenBy(s => s.Branch.Name),
                (_, "branchname",  true)  => ordered!.ThenByDescending(s => s.Branch.Name),
                _ => ordered!.ThenByDescending(s => s.CreatedAt)
            };
        }

        return ordered ?? query.OrderByDescending(s => s.CreatedAt);
    }

    /// <summary>
    /// Deletes a sale from the database
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await GetByIdAsync(id, cancellationToken);
        if (sale == null)
            return false;

        _context.Sales.Remove(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
