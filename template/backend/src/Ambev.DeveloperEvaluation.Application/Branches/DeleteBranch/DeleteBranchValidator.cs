using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

/// <summary>
/// Validator for DeleteBranchCommand
/// </summary>
public class DeleteBranchValidator : AbstractValidator<DeleteBranchCommand>
{
    public DeleteBranchValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Branch ID is required");
    }
}
