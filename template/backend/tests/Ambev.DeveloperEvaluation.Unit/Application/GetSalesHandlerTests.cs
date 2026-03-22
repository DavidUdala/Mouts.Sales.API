using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="GetSalesHandler"/> class.
/// </summary>
public class GetSalesHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetSalesHandler> _logger;
    private readonly GetSalesHandler _handler;

    public GetSalesHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<GetSalesHandler>>();
        _handler = new GetSalesHandler(_saleRepository, _mapper, _logger);
    }

    [Fact(DisplayName = "Given valid query When getting sales Then returns paginated result")]
    public async Task Handle_ValidQuery_ReturnsPaginatedResult()
    {
        // Given
        var query = SaleHandlerTestData.GenerateValidGetSalesQuery();
        var sales = new List<Sale> { SaleHandlerTestData.GenerateValidSale(), SaleHandlerTestData.GenerateValidSale() };
        var totalCount = sales.Count;

        _saleRepository.GetSalesAsync(
            page: query.Page, size: query.Size,
            customerName: null, branchName: null, saleNumber: null,
            isCancelled: null, minDate: null, maxDate: null,
            minTotal: null, maxTotal: null,
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns((sales.AsEnumerable(), totalCount));

        _mapper.Map<IEnumerable<GetSalesItemResult>>(Arg.Any<IEnumerable<Sale>>())
            .Returns(sales.Select(s => new GetSalesItemResult { Id = s.Id }));

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.TotalCount.Should().Be(totalCount);
        result.Page.Should().Be(query.Page);
        result.PageSize.Should().Be(query.Size);
        result.Items.Should().HaveCount(totalCount);
    }

    [Fact(DisplayName = "Given valid query When getting sales Then repository is called with correct parameters")]
    public async Task Handle_ValidQuery_CallsRepositoryWithCorrectParameters()
    {
        // Given
        var query = new GetSalesQuery { Page = 2, Size = 5, CustomerName = "Ambev*" };

        _saleRepository.GetSalesAsync(
            page: 2, size: 5,
            customerName: "Ambev*", branchName: null, saleNumber: null,
            isCancelled: null, minDate: null, maxDate: null,
            minTotal: null, maxTotal: null,
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns((Enumerable.Empty<Sale>(), 0));

        _mapper.Map<IEnumerable<GetSalesItemResult>>(Arg.Any<IEnumerable<Sale>>())
            .Returns(Enumerable.Empty<GetSalesItemResult>());

        // When
        await _handler.Handle(query, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).GetSalesAsync(
            page: 2, size: 5,
            customerName: "Ambev*", branchName: null, saleNumber: null,
            isCancelled: null, minDate: null, maxDate: null,
            minTotal: null, maxTotal: null,
            cancellationToken: Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given query with no results When getting sales Then returns empty list with zero total")]
    public async Task Handle_NoResults_ReturnsEmptyListWithZeroTotal()
    {
        // Given
        var query = SaleHandlerTestData.GenerateValidGetSalesQuery();

        _saleRepository.GetSalesAsync(
            page: Arg.Any<int>(), size: Arg.Any<int>(),
            customerName: Arg.Any<string?>(), branchName: Arg.Any<string?>(),
            saleNumber: Arg.Any<string?>(), isCancelled: Arg.Any<bool?>(),
            minDate: Arg.Any<DateTime?>(), maxDate: Arg.Any<DateTime?>(),
            minTotal: Arg.Any<decimal?>(), maxTotal: Arg.Any<decimal?>(),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns((Enumerable.Empty<Sale>(), 0));

        _mapper.Map<IEnumerable<GetSalesItemResult>>(Arg.Any<IEnumerable<Sale>>())
            .Returns(Enumerable.Empty<GetSalesItemResult>());

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.TotalCount.Should().Be(0);
        result.Items.Should().BeEmpty();
    }
}
