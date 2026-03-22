using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Rebus.Bus;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CancelSaleHandler"/> class.
/// </summary>
public class CancelSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IBus _bus;
    private readonly ILogger<CancelSaleHandler> _logger;
    private readonly IMapper _mapper;
    private readonly CancelSaleHandler _handler;

    public CancelSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _bus = Substitute.For<IBus>();
        _logger = Substitute.For<ILogger<CancelSaleHandler>>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CancelSaleHandler(_saleRepository, _bus, _logger, _mapper);
    }

    [Fact(DisplayName = "Given valid sale ID When cancelling sale Then sale is cancelled and result is returned")]
    public async Task Handle_ValidSaleId_ReturnsCancelResult()
    {
        // Given
        var sale = SaleHandlerTestData.GenerateValidSale();
        var command = SaleHandlerTestData.GenerateValidCancelCommand(sale.Id);
        var expectedResult = new CancelSaleResult { Id = sale.Id, IsCancelled = true };

        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CancelSaleResult>(Arg.Any<Sale>()).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.IsCancelled.Should().BeTrue();
    }

    [Fact(DisplayName = "Given valid sale ID When cancelling sale Then publishes SaleCancelledEvent")]
    public async Task Handle_ValidSaleId_PublishesSaleCancelledEvent()
    {
        // Given
        var sale = SaleHandlerTestData.GenerateValidSale();
        var command = SaleHandlerTestData.GenerateValidCancelCommand(sale.Id);

        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CancelSaleResult>(Arg.Any<Sale>()).Returns(new CancelSaleResult());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _bus.Received(1).Publish(Arg.Any<object>());
    }

    [Fact(DisplayName = "Given valid sale ID When cancelling sale Then UpdateAsync is called once")]
    public async Task Handle_ValidSaleId_CallsUpdateAsyncOnce()
    {
        // Given
        var sale = SaleHandlerTestData.GenerateValidSale();
        var command = SaleHandlerTestData.GenerateValidCancelCommand(sale.Id);

        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CancelSaleResult>(Arg.Any<Sale>()).Returns(new CancelSaleResult());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given non-existent sale ID When cancelling Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentSaleId_ThrowsKeyNotFoundException()
    {
        // Given
        var saleId = Guid.NewGuid();
        var command = SaleHandlerTestData.GenerateValidCancelCommand(saleId);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).ReturnsNull();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*{saleId}*");
    }

    [Fact(DisplayName = "Given empty sale ID When cancelling Then throws ValidationException")]
    public async Task Handle_EmptySaleId_ThrowsValidationException()
    {
        // Given
        var command = new CancelSaleCommand(Guid.Empty);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }
}
