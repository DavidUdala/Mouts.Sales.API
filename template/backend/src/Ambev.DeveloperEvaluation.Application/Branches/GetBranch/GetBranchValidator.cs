using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranch;

/// <summary>
/// Validator for GetBranchCommand
/// </summary>
public class GetBranchValidator : AbstractValidator<GetBranchCommand>
{
    public GetBranchValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Branch ID is required");
    }
}
