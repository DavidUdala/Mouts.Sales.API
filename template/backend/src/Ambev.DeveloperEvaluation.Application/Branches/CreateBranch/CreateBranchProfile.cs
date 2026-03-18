using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

/// <summary>
/// Profile for mapping between Branch entity and CreateBranch command/result
/// </summary>
public class CreateBranchProfile : Profile
{
    public CreateBranchProfile()
    {
        CreateMap<CreateBranchCommand, Branch>();
        CreateMap<Branch, CreateBranchResult>();
    }
}
