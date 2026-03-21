using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// AutoMapper profile for the GetSales WebApi feature.
/// </summary>
public class GetSalesProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for the GetSales WebApi feature.
    /// </summary>
    public GetSalesProfile()
    {
        CreateMap<GetSaleRequest, GetSalesQuery>();

        CreateMap<GetSalesItemResult, GetSaleResponse>();
        CreateMap<GetSalesLineItemResult, GetSaleItemResponse>();
    }
}
