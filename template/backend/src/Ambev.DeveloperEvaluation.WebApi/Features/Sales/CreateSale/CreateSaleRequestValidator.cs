using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    /// <summary>
    /// Validator for CreateSaleRequest that defines validation rules for sale creation.
    /// </summary>
    public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
    {
        /// <summary>
        /// Initializes a new instance of the CreateUserRequestValidator with defined validation rules.
        /// </summary>
        /// Validation rules include:
        /// - CustomerId: Required
        /// - BranchId: Required
        /// - Items: Required
        ///     - ProductId: Required
        ///     - Quantity: Must be greater than zero
        ///     - UnitPrice: Must be greater than zero, should have at most 2 decimal places
        /// </remarks>
        public CreateSaleRequestValidator()
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
}