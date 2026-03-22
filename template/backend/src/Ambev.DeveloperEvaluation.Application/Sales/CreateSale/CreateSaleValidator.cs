using Ambev.DeveloperEvaluation.Application.SaleItem.CreateSaleItem;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleCommand that defines validation rules for sale creation.
/// </summary>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleCommandValidator with defined validation rules.
    /// </summary>
    public CreateSaleCommandValidator()
    {
        RuleFor(sale => sale.CustomerId).NotEmpty();
        RuleFor(sale => sale.BranchId).NotEmpty();
        RuleFor(sale => sale.Items).NotEmpty().WithMessage("Sale must have at least one product.");

        RuleForEach(sale => sale.Items)
            .ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId).NotEmpty();
                item.RuleFor(i => i.Quantity).GreaterThan(0).LessThanOrEqualTo(20).WithMessage("Quantity must be greater than 0 and less than or equal to 20.");
                item.RuleFor(i => i.UnitPrice).GreaterThan(0).Must(price => decimal.Round(price, 2) == price).WithMessage("Unit price must have exactly 2 decimal places.");
            });
    }
}