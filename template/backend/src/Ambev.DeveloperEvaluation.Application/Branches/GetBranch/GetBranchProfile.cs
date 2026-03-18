using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranch;

/// <summary>
/// Profile for mapping between Branch entity and GetBranchResult
/// </summary>
public class GetBranchProfile : Profile
{
    public GetBranchProfile()
    {
        CreateMap<Branch, GetBranchResult>();
    }
}
