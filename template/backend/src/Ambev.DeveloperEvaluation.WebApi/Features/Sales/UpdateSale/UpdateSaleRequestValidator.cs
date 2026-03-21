using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
    {
        public UpdateSaleRequestValidator()
        {
            RuleFor(sale => sale.Id).NotEmpty();
            RuleFor(sale => sale.Item)
                .ChildRules(item => 
                {
                    item.RuleFor(i => i.Id).NotEmpty();  
                    item.RuleFor(i => i.Quantity).GreaterThan(0).LessThanOrEqualTo(20).WithMessage("Quantity must be greater than 0 and less than or equal to 20.");
                    item.RuleFor(i => i.UnitPrice).GreaterThan(0).Must(price => decimal.Round(price, 2) == price).WithMessage("Unit price must have exactly 2 decimal places.");
                });
        }
    }
}