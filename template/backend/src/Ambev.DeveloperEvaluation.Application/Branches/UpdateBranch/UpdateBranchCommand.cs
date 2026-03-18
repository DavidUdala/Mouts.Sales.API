using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

/// <summary>
/// Command for updating an existing branch.
/// </summary>
public class UpdateBranchCommand : IRequest<UpdateBranchResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the branch to update.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the branch.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the address of the branch.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    public ValidationResultDetail Validate()
    {
        var validator = new UpdateBranchCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}
