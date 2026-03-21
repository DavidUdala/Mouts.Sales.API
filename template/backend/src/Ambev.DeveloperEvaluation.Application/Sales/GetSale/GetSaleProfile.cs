using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.SaleItem.GetSaleItem;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Profile for mapping between Sale entity and GetSaleResult
/// </summary>
public class GetSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetSale operation
    /// </summary>
    public GetSaleProfile()
    {
        CreateMap<Sale, GetSaleResult>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Username))
            .ForMember(dest => dest.BranchName,   opt => opt.MapFrom(src => src.Branch.Name));

        CreateMap<Domain.Entities.SaleItem, GetSaleItemResult>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
    }
}
