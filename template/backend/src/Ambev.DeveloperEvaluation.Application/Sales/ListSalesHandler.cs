using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public sealed class ListSalesHandler : IRequestHandler<ListSalesQuery, PagedSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public ListSalesHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<PagedSaleResult> Handle(
        ListSalesQuery query,
        CancellationToken cancellationToken)
    {
        if (query.Page < 1 || query.PageSize < 1 || query.PageSize > 100)
            return new()
            {
                CurrentPage = query.Page,
                PageSize = query.PageSize
            };

        var result = await _saleRepository.GetPagedAsync(
            query.Page,
            query.PageSize,
            cancellationToken);

        return new()
        {
            Items = _mapper.Map<IReadOnlyCollection<SaleResult>>(result.Items),
            CurrentPage = query.Page,
            PageSize = query.PageSize,
            TotalCount = result.TotalCount,
            TotalPages = (int)Math.Ceiling(result.TotalCount / (double)query.PageSize)
        };
    }
}
