using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public sealed class GetSaleHandler : IRequestHandler<GetSaleQuery, CommandResult<SaleResult>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public GetSaleHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<CommandResult<SaleResult>> Handle(
        GetSaleQuery query,
        CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(query.Id, cancellationToken);

        return sale is null
            ? CommandResultFactory.NotFound<SaleResult>("sale.not_found", "Sale not found.")
            : CommandResultFactory.Success(_mapper.Map<SaleResult>(sale));
    }
}
