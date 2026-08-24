using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

public sealed class SalesProfile : Profile
{
    public SalesProfile()
    {
        CreateMap<SaleRequest, CreateSaleCommand>();
        CreateMap<SaleRequest, UpdateSaleCommand>();
        CreateMap<SaleItemRequest, SaleItemInput>();
    }
}
