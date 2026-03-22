using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Validator for UpdateSaleCommand that defines validation rules for sale update.
/// </summary>
public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateSaleCommandValidator with defined validation rules.
    /// </summary>
    public UpdateSaleCommandValidator()
    {
        RuleFor(sale => sale.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");

        RuleFor(sale => sale.Item)
            .NotNull()
            .WithMessage("Item is required");

        RuleFor(sale => sale.Item.Id)
            .NotEmpty()
            .WithMessage("Item ID is required");

        RuleFor(sale => sale.Item.Quantity)
            .GreaterThan(0)
            .LessThanOrEqualTo(20)
            .WithMessage("Quantity must be between 1 and 20");

        RuleFor(sale => sale.Item.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than zero");
    }
}
