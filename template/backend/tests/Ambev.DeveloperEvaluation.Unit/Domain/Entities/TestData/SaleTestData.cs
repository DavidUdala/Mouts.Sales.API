using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for Sale and SaleItem entities using the Bogus library.
/// Centralizes test data creation to ensure consistency across domain test cases.
/// </summary>
public static class SaleTestData
{
    private static readonly Faker Faker = new();

    /// <summary>
    /// Generates a valid Sale with the specified number of items and quantity per item.
    /// Items share the same ProductId to allow discount tier testing.
    /// </summary>
    public static Sale GenerateValidSale(int itemCount = 1, int quantityPerItem = 1)
    {
        var productId = Guid.NewGuid();
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            IsCancelled = false
        };

        for (var i = 0; i < itemCount; i++)
            sale.Items.Add(GenerateSaleItem(productId: productId, quantity: quantityPerItem));

        sale.GenerateSaleNumber();
        return sale;
    }

    /// <summary>
    /// Generates a Sale with a single item whose total quantity falls in a given discount tier.
    /// </summary>
    public static Sale GenerateSaleWithQuantity(int totalQuantity)
    {
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            IsCancelled = false
        };

        sale.Items.Add(GenerateSaleItem(quantity: totalQuantity));
        sale.GenerateSaleNumber();
        return sale;
    }

    /// <summary>
    /// Generates a cancelled Sale with all items marked as cancelled.
    /// </summary>
    public static Sale GenerateCancelledSale()
    {
        var sale = GenerateValidSale(itemCount: 1, quantityPerItem: 1);
        sale.IsCancelled = true;
        foreach (var item in sale.Items)
            item.IsCancelled = true;
        return sale;
    }

    /// <summary>
    /// Generates a SaleItem with configurable properties.
    /// </summary>
    public static SaleItem GenerateSaleItem(
        Guid? id = null,
        Guid? productId = null,
        int quantity = 1,
        decimal unitPrice = 10m,
        bool isCancelled = false)
    {
        return new SaleItem
        {
            Id = id ?? Guid.NewGuid(),
            ProductId = productId ?? Guid.NewGuid(),
            Quantity = quantity,
            UnitPrice = unitPrice,
            Discount = 0m,
            IsCancelled = isCancelled
        };
    }
}
