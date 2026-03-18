using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Validator for CreateProductCommand that defines validation rules for product creation.
/// </summary>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(product => product.Name)
            .NotEmpty()
            .WithMessage("Product name is required")
            .MaximumLength(200)
            .WithMessage("Product name cannot exceed 200 characters");

        RuleFor(product => product.Description)
            .MaximumLength(1000)
            .WithMessage("Product description cannot exceed 1000 characters");

        RuleFor(product => product.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than zero");
    }
}
