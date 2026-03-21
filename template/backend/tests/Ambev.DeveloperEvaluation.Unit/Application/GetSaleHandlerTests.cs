using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="GetSaleHandler"/> class.
/// </summary>
public class GetSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetSaleHandler> _logger;
    private readonly GetSaleHandler _handler;

    public GetSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<GetSaleHandler>>();
        _handler = new GetSaleHandler(_saleRepository, _mapper, _logger);
    }

    [Fact(DisplayName = "Given existing sale ID When getting sale Then returns sale result")]
    public async Task Handle_ExistingSaleId_ReturnsSaleResult()
    {
        // Given
        var sale = SaleHandlerTestData.GenerateValidSale();
        var command = SaleHandlerTestData.GenerateValidGetCommand(sale.Id);
        var result = new GetSaleResult { Id = sale.Id, SaleNumber = sale.SaleNumber };

        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(result);

        // When
        var getSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        getSaleResult.Should().NotBeNull();
        getSaleResult.Id.Should().Be(sale.Id);
    }

    [Fact(DisplayName = "Given existing sale ID When getting sale Then repository is called once")]
    public async Task Handle_ExistingSaleId_CallsRepositoryOnce()
    {
        // Given
        var sale = SaleHandlerTestData.GenerateValidSale();
        var command = SaleHandlerTestData.GenerateValidGetCommand(sale.Id);

        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(new GetSaleResult());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).GetByIdAsync(sale.Id, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given non-existent sale ID When getting sale Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentSaleId_ThrowsKeyNotFoundException()
    {
        // Given
        var saleId = Guid.NewGuid();
        var command = SaleHandlerTestData.GenerateValidGetCommand(saleId);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).ReturnsNull();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*{saleId}*");
    }
}
