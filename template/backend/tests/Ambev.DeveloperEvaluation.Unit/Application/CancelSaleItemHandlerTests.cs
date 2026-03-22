using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Rebus.Bus;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CancelSaleItemHandler"/> class.
/// </summary>
public class CancelSaleItemHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IBus _bus;
    private readonly ILogger<CancelSaleItemHandler> _logger;
    private readonly CancelSaleItemHandler _handler;

    public CancelSaleItemHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _bus = Substitute.For<IBus>();
        _logger = Substitute.For<ILogger<CancelSaleItemHandler>>();
        _handler = new CancelSaleItemHandler(_saleRepository, _bus, _logger);
    }

    [Fact(DisplayName = "Given valid sale and item IDs When cancelling item Then returns success result")]
    public async Task Handle_ValidIds_ReturnsCancelItemResult()
    {
        // Given
        var sale = SaleHandlerTestData.GenerateValidSale();
        var item = sale.Items.First();
        var command = SaleHandlerTestData.GenerateValidCancelItemCommand(sale.Id, item.Id);

        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.SaleId.Should().Be(sale.Id);
        result.ItemId.Should().Be(item.Id);
        result.IsCancelled.Should().BeTrue();
    }

    [Fact(DisplayName = "Given valid IDs When cancelling item Then publishes ItemCancelledEvent")]
    public async Task Handle_ValidIds_PublishesItemCancelledEvent()
    {
        // Given
        var sale = SaleHandlerTestData.GenerateValidSale();
        var item = sale.Items.First();
        var command = SaleHandlerTestData.GenerateValidCancelItemCommand(sale.Id, item.Id);

        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _bus.Received(1).Publish(Arg.Any<object>());
    }

    [Fact(DisplayName = "Given non-existent sale ID When cancelling item Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentSaleId_ThrowsKeyNotFoundException()
    {
        // Given
        var command = SaleHandlerTestData.GenerateValidCancelItemCommand(Guid.NewGuid(), Guid.NewGuid());

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).ReturnsNull();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*{command.SaleId}*");
    }

    [Fact(DisplayName = "Given cancelled sale When cancelling item Then throws InvalidOperationException")]
    public async Task Handle_CancelledSale_ThrowsInvalidOperationException()
    {
        // Given
        var sale = SaleHandlerTestData.GenerateCancelledSale();
        var command = SaleHandlerTestData.GenerateValidCancelItemCommand(sale.Id, Guid.NewGuid());

        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>()).Returns(sale);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact(DisplayName = "Given non-existent item ID When cancelling Then throws DomainException")]
    public async Task Handle_NonExistentItemId_ThrowsDomainException()
    {
        // Given
        var sale = SaleHandlerTestData.GenerateValidSale();
        var command = SaleHandlerTestData.GenerateValidCancelItemCommand(sale.Id, Guid.NewGuid());

        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>()).Returns(sale);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<DomainException>();
    }

    [Fact(DisplayName = "Given valid IDs When cancelling item Then UpdateAsync is called once")]
    public async Task Handle_ValidIds_CallsUpdateAsyncOnce()
    {
        // Given
        var sale = SaleHandlerTestData.GenerateValidSale();
        var item = sale.Items.First();
        var command = SaleHandlerTestData.GenerateValidCancelItemCommand(sale.Id, item.Id);

        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }
}
