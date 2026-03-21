using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Sale domain entity.
/// Covers business rules for sale number generation, discounts, totals, cancellation, and item management.
/// </summary>
public class SaleTests
{
    [Fact(DisplayName = "Given new sale When generating sale number Then format is SALE-YYYYMMDD-XXXXXXXX")]
    public void Given_NewSale_When_GenerateSaleNumber_Then_FormatIsCorrect()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        sale.GenerateSaleNumber();

        // Assert
        Assert.False(string.IsNullOrEmpty(sale.SaleNumber));
        Assert.Matches(@"^SALE-\d{8}-[A-F0-9]{8}$", sale.SaleNumber);
    }

    [Fact(DisplayName = "Given two sales When generating sale numbers Then numbers are unique")]
    public void Given_TwoSales_When_GenerateSaleNumbers_Then_NumbersAreUnique()
    {
        // Arrange
        var sale1 = SaleTestData.GenerateValidSale();
        var sale2 = SaleTestData.GenerateValidSale();

        // Act
        sale1.GenerateSaleNumber();
        sale2.GenerateSaleNumber();

        // Assert
        Assert.NotEqual(sale1.SaleNumber, sale2.SaleNumber);
    }

    [Fact(DisplayName = "Given item quantity below 4 When applying discounts Then discount is 0%")]
    public void Given_QuantityBelow4_When_ApplyDiscounts_Then_DiscountIsZero()
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithQuantity(3);

        // Act
        sale.ApplyDiscounts();

        // Assert
        Assert.Equal(0m, sale.Items.First().Discount);
    }

    [Theory(DisplayName = "Given item quantity between 4 and 9 When applying discounts Then discount is 10%")]
    [InlineData(4)]
    [InlineData(7)]
    [InlineData(9)]
    public void Given_QuantityBetween4And9_When_ApplyDiscounts_Then_DiscountIs10Percent(int quantity)
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithQuantity(quantity);

        // Act
        sale.ApplyDiscounts();

        // Assert
        Assert.Equal(10m, sale.Items.First().Discount);
    }

    [Theory(DisplayName = "Given item quantity between 10 and 20 When applying discounts Then discount is 20%")]
    [InlineData(10)]
    [InlineData(15)]
    [InlineData(20)]
    public void Given_QuantityBetween10And20_When_ApplyDiscounts_Then_DiscountIs20Percent(int quantity)
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithQuantity(quantity);

        // Act
        sale.ApplyDiscounts();

        // Assert
        Assert.Equal(20m, sale.Items.First().Discount);
    }

    [Theory(DisplayName = "Given item quantity above 20 When applying discounts Then throws DomainException")]
    [InlineData(21)]
    [InlineData(50)]
    public void Given_QuantityAbove20_When_ApplyDiscounts_Then_ThrowsDomainException(int quantity)
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithQuantity(quantity);

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.ApplyDiscounts());
    }

    [Fact(DisplayName = "Given items without discount When calculating totals Then total is quantity times unit price")]
    public void Given_ItemsWithoutDiscount_When_CalculateTotals_Then_TotalIsQuantityTimesUnitPrice()
    {
        // Arrange
        var item = SaleTestData.GenerateSaleItem(quantity: 3, unitPrice: 100m);
        var sale = SaleTestData.GenerateValidSale();
        sale.Items.Clear();
        sale.Items.Add(item);

        // Act
        sale.CalculateTotals();

        // Assert
        Assert.Equal(300m, item.TotalAmount);
        Assert.Equal(300m, sale.TotalAmount);
    }

    [Fact(DisplayName = "Given items with 10% discount When calculating totals Then total reflects discount")]
    public void Given_ItemsWith10PercentDiscount_When_CalculateTotals_Then_TotalReflectsDiscount()
    {
        // Arrange
        var item = SaleTestData.GenerateSaleItem(quantity: 5, unitPrice: 100m);
        item.Discount = 10m;
        var sale = SaleTestData.GenerateValidSale();
        sale.Items.Clear();
        sale.Items.Add(item);

        // Act
        sale.CalculateTotals();

        // Assert
        Assert.Equal(450m, item.TotalAmount); // 5 * 100 * (1 - 0.10)
        Assert.Equal(450m, sale.TotalAmount);
    }

    [Fact(DisplayName = "Given cancelled item When calculating totals Then cancelled item is not recalculated")]
    public void Given_CancelledItem_When_CalculateTotals_Then_CancelledItemNotRecalculated()
    {
        // Arrange
        var activeItem = SaleTestData.GenerateSaleItem(quantity: 2, unitPrice: 50m);
        var cancelledItem = SaleTestData.GenerateSaleItem(quantity: 3, unitPrice: 100m, isCancelled: true);
        var sale = SaleTestData.GenerateValidSale();
        sale.Items.Clear();
        sale.Items.Add(activeItem);
        sale.Items.Add(cancelledItem);

        // Act
        sale.CalculateTotals();

        // Assert
        Assert.Equal(100m, activeItem.TotalAmount); // 2 * 50
        Assert.Equal(0m, cancelledItem.TotalAmount); // not recalculated
    }

    [Fact(DisplayName = "Given active sale When cancelling Then sale is marked cancelled")]
    public void Given_ActiveSale_When_Cancel_Then_IsCancelledIsTrue()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale(itemCount: 2);

        // Act
        sale.Cancel();

        // Assert
        Assert.True(sale.IsCancelled);
    }

    [Fact(DisplayName = "Given active sale When cancelling Then all items are cancelled")]
    public void Given_ActiveSale_When_Cancel_Then_AllItemsAreCancelled()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale(itemCount: 3);

        // Act
        sale.Cancel();

        // Assert
        foreach (var item in sale.Items)
            Assert.True(item.IsCancelled);
    }

    [Fact(DisplayName = "Given active sale When cancelling Then UpdatedAt is set")]
    public void Given_ActiveSale_When_Cancel_Then_UpdatedAtIsSet()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var before = DateTime.UtcNow;

        // Act
        sale.Cancel();

        // Assert
        Assert.NotNull(sale.UpdatedAt);
        Assert.True(sale.UpdatedAt >= before);
    }

    [Fact(DisplayName = "Given already cancelled sale When cancelling again Then throws DomainException")]
    public void Given_CancelledSale_When_CancelAgain_Then_ThrowsDomainException()
    {
        // Arrange
        var sale = SaleTestData.GenerateCancelledSale();

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.Cancel());
    }

    [Fact(DisplayName = "Given sale with item When cancelling item Then item is marked cancelled")]
    public void Given_SaleWithItem_When_CancelItem_Then_ItemIsCancelled()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var item = SaleTestData.GenerateSaleItem(id: itemId, quantity: 1, unitPrice: 50m);
        var sale = SaleTestData.GenerateValidSale();
        sale.Items.Clear();
        sale.Items.Add(item);

        // Act
        sale.CancelItem(itemId);

        // Assert
        Assert.True(sale.Items.First(i => i.Id == itemId).IsCancelled);
    }

    [Fact(DisplayName = "Given sale When cancelling item Then active item total is recalculated")]
    public void Given_Sale_When_CancelItem_Then_ActiveItemTotalIsRecalculated()
    {
        // Arrange
        var itemToCancel = SaleTestData.GenerateSaleItem(id: Guid.NewGuid(), quantity: 2, unitPrice: 100m);
        var remainingItem = SaleTestData.GenerateSaleItem(quantity: 1, unitPrice: 50m);
        var sale = SaleTestData.GenerateValidSale();
        sale.Items.Clear();
        sale.Items.Add(itemToCancel);
        sale.Items.Add(remainingItem);

        // Act
        sale.CancelItem(itemToCancel.Id);

        // Assert
        Assert.Equal(50m, remainingItem.TotalAmount);
        Assert.Equal(50m, sale.Items.Where(i => !i.IsCancelled).Sum(i => i.TotalAmount));
    }

    [Fact(DisplayName = "Given sale When cancelling non-existent item Then throws DomainException")]
    public void Given_Sale_When_CancelNonExistentItem_Then_ThrowsDomainException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.CancelItem(Guid.NewGuid()));
    }

    [Fact(DisplayName = "Given sale with item When updating item Then quantity and price are updated")]
    public void Given_SaleWithItem_When_UpdateItem_Then_QuantityAndPriceAreUpdated()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var item = SaleTestData.GenerateSaleItem(id: itemId, quantity: 1, unitPrice: 10m);
        var sale = SaleTestData.GenerateValidSale();
        sale.Items.Clear();
        sale.Items.Add(item);

        // Act
        sale.UpdateItem(itemId, quantity: 3, unitPrice: 25m);

        // Assert
        var updatedItem = sale.Items.First(i => i.Id == itemId);
        Assert.Equal(3, updatedItem.Quantity);
        Assert.Equal(25m, updatedItem.UnitPrice);
    }

    [Fact(DisplayName = "Given sale When updating item Then UpdatedAt is refreshed")]
    public void Given_Sale_When_UpdateItem_Then_UpdatedAtIsRefreshed()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var item = SaleTestData.GenerateSaleItem(id: itemId, quantity: 1, unitPrice: 10m);
        var sale = SaleTestData.GenerateValidSale();
        sale.Items.Clear();
        sale.Items.Add(item);
        var before = DateTime.UtcNow;

        // Act
        sale.UpdateItem(itemId, quantity: 2, unitPrice: 20m);

        // Assert
        Assert.NotNull(sale.UpdatedAt);
        Assert.True(sale.UpdatedAt >= before);
    }

    [Fact(DisplayName = "Given sale When updating non-existent item Then throws DomainException")]
    public void Given_Sale_When_UpdateNonExistentItem_Then_ThrowsDomainException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.UpdateItem(Guid.NewGuid(), quantity: 2, unitPrice: 20m));
    }

    [Fact(DisplayName = "Given sale When updating cancelled item Then throws DomainException")]
    public void Given_Sale_When_UpdateCancelledItem_Then_ThrowsDomainException()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var item = SaleTestData.GenerateSaleItem(id: itemId, isCancelled: true);
        var sale = SaleTestData.GenerateValidSale();
        sale.Items.Clear();
        sale.Items.Add(item);

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.UpdateItem(itemId, quantity: 2, unitPrice: 20m));
    }
}
