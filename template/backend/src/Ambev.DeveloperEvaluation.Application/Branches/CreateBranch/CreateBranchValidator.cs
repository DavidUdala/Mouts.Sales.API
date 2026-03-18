using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

/// <summary>
/// Validator for CreateBranchCommand that defines validation rules for branch creation.
/// </summary>
public class CreateBranchCommandValidator : AbstractValidator<CreateBranchCommand>
{
    public CreateBranchCommandValidator()
    {
        RuleFor(branch => branch.Name)
            .NotEmpty()
            .WithMessage("Branch name is required")
            .MaximumLength(200)
            .WithMessage("Branch name cannot exceed 200 characters");

        RuleFor(branch => branch.Address)
            .MaximumLength(500)
            .WithMessage("Branch address cannot exceed 500 characters");
    }
}
