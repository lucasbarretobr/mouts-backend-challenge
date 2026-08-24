using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public sealed class SalesApplicationProfile : Profile
{
    public SalesApplicationProfile()
    {
        CreateMap<SaleItemInput, SaleItem>();
        CreateMap<Sale, SaleResult>();
        CreateMap<SaleItem, SaleItemResult>();
    }
}
