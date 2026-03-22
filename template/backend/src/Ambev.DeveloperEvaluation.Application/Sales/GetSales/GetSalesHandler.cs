using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Handler for processing GetSalesQuery requests.
/// </summary>
public class GetSalesHandler : IRequestHandler<GetSalesQuery, GetSalesResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetSalesHandler> _logger;

    /// <summary>
    /// Initializes a new instance of GetSalesHandler.
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger instance</param>
    public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<GetSalesHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the GetSalesQuery request.
    /// </summary>
    /// <param name="query">The GetSales query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A paginated list of sales matching the filters</returns>
    public async Task<GetSalesResult> Handle(GetSalesQuery query, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _saleRepository.GetSalesAsync(
            page:         query.Page,
            size:         query.Size,
            customerName: query.CustomerName,
            branchName:   query.BranchName,
            saleNumber:   query.SaleNumber,
            isCancelled:  query.IsCancelled,
            minDate:      query.MinDate,
            maxDate:      query.MaxDate,
            minTotal:     query.MinTotal,
            maxTotal:     query.MaxTotal,
            order:        query.Order,
            cancellationToken: cancellationToken
        );

        _logger.LogInformation(
            "Retrieved {TotalCount} sales for page {Page} with size {PageSize}",
            totalCount,
            query.Page,
            query.Size);

        return new GetSalesResult
        {
            Items     = _mapper.Map<IEnumerable<GetSalesItemResult>>(items),
            TotalCount = totalCount,
            Page      = query.Page,
            PageSize  = query.Size
        };
    }
}
