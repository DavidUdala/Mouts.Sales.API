using Ambev.DeveloperEvaluation.Application.SaleItem.CreateSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

/// <summary>
/// Provides methods for generating test data for all Sale command and query handlers.
/// Centralizes faker configuration to ensure consistent and realistic test data across handler tests.
/// </summary>
public static class SaleHandlerTestData
{
    private static readonly Faker<CreateSaleItemCommand> CreateItemFaker =
        new Faker<CreateSaleItemCommand>()
            .RuleFor(i => i.ProductId, f => Guid.NewGuid())
            .RuleFor(i => i.Quantity, f => f.Random.Int(1, 3))
            .RuleFor(i => i.UnitPrice, f => Math.Round(f.Random.Decimal(5, 200), 2))
            .RuleFor(i => i.Discount, 0m);

    private static readonly Faker<CreateSaleCommand> CreateCommandFaker =
        new Faker<CreateSaleCommand>()
            .RuleFor(c => c.CustomerId, f => Guid.NewGuid())
            .RuleFor(c => c.BranchId, f => Guid.NewGuid())
            .RuleFor(c => c.Items, f => CreateItemFaker.Generate(f.Random.Int(1, 3)));

    private static readonly Faker<SaleItem> SaleItemFaker =
        new Faker<SaleItem>()
            .RuleFor(i => i.Id, f => Guid.NewGuid())
            .RuleFor(i => i.ProductId, f => Guid.NewGuid())
            .RuleFor(i => i.Quantity, f => f.Random.Int(1, 3))
            .RuleFor(i => i.UnitPrice, f => Math.Round(f.Random.Decimal(5, 200), 2))
            .RuleFor(i => i.Discount, 0m)
            .RuleFor(i => i.IsCancelled, false);

    private static readonly Faker<Sale> SaleFaker =
        new Faker<Sale>()
            .RuleFor(s => s.Id, f => Guid.NewGuid())
            .RuleFor(s => s.SaleNumber, f => $"SALE-{DateTime.UtcNow:yyyyMMdd}-{f.Random.AlphaNumeric(8).ToUpperInvariant()}")
            .RuleFor(s => s.CustomerId, f => Guid.NewGuid())
            .RuleFor(s => s.BranchId, f => Guid.NewGuid())
            .RuleFor(s => s.TotalAmount, f => Math.Round(f.Random.Decimal(50, 500), 2))
            .RuleFor(s => s.IsCancelled, false)
            .RuleFor(s => s.Items, f => SaleItemFaker.Generate(f.Random.Int(1, 3)));

    /// <summary>Generates a valid CreateSaleCommand.</summary>
    public static CreateSaleCommand GenerateValidCreateCommand() =>
        CreateCommandFaker.Generate();

    /// <summary>Generates a valid Sale entity as would be returned by the repository.</summary>
    public static Sale GenerateValidSale() => SaleFaker.Generate();

    /// <summary>Generates a cancelled Sale entity.</summary>
    public static Sale GenerateCancelledSale()
    {
        var sale = SaleFaker.Generate();
        sale.IsCancelled = true;
        return sale;
    }

    /// <summary>Generates a valid UpdateSaleCommand for a given sale and item ID.</summary>
    public static UpdateSaleCommand GenerateValidUpdateCommand(Guid saleId, Guid itemId) =>
        new()
        {
            Id = saleId,
            Item = new UpdateSaleItemCommand
            {
                Id = itemId,
                Quantity = new Faker().Random.Int(1, 9),
                UnitPrice = Math.Round(new Faker().Random.Decimal(5, 200), 2)
            }
        };

    /// <summary>Generates a valid CancelSaleCommand.</summary>
    public static CancelSaleCommand GenerateValidCancelCommand(Guid saleId) =>
        new(saleId);

    /// <summary>Generates a valid CancelSaleItemCommand.</summary>
    public static CancelSaleItemCommand GenerateValidCancelItemCommand(Guid saleId, Guid itemId) =>
        new(saleId, itemId);

    /// <summary>Generates a valid GetSaleCommand.</summary>
    public static GetSaleCommand GenerateValidGetCommand(Guid saleId) =>
        new(saleId);

    /// <summary>Generates a valid GetSalesQuery with pagination defaults.</summary>
    public static GetSalesQuery GenerateValidGetSalesQuery() =>
        new() { Page = 1, Size = 10 };
}
