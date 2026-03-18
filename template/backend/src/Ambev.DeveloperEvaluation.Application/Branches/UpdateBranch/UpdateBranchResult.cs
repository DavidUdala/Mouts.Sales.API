namespace Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

/// <summary>
/// Represents the response returned after successfully updating a branch.
/// </summary>
public class UpdateBranchResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the updated branch.
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
}
