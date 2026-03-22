using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// AutoMapper profile for GetSales operation.
/// </summary>
public class GetSalesProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for the GetSales operation.
    /// </summary>
    public GetSalesProfile()
    {
        CreateMap<Sale, GetSalesItemResult>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Username))
            .ForMember(dest => dest.BranchName,   opt => opt.MapFrom(src => src.Branch.Name));

        CreateMap<Domain.Entities.SaleItem, GetSalesLineItemResult>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
    }
}
