using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

/// <summary>
/// Profile for mapping between Branch entity and UpdateBranch command/result
/// </summary>
public class UpdateBranchProfile : Profile
{
    public UpdateBranchProfile()
    {
        CreateMap<UpdateBranchCommand, Branch>();
        CreateMap<Branch, UpdateBranchResult>();
    }
}
