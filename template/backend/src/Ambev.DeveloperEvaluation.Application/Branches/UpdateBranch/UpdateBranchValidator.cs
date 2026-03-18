using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

/// <summary>
/// Validator for UpdateBranchCommand that defines validation rules for branch update.
/// </summary>
public class UpdateBranchCommandValidator : AbstractValidator<UpdateBranchCommand>
{
    public UpdateBranchCommandValidator()
    {
        RuleFor(branch => branch.Id)
            .NotEmpty()
            .WithMessage("Branch ID is required");

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
