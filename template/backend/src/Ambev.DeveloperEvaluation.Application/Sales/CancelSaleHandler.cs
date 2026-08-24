using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Common.Messaging;
using Ambev.DeveloperEvaluation.Application.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public sealed class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CommandResult<SaleResult>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _eventPublisher;

    public CancelSaleHandler(ISaleRepository saleRepository, IMapper mapper, IEventPublisher eventPublisher)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<CommandResult<SaleResult>> Handle(
        CancelSaleCommand command,
        CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.CancelAsync(command.Id, cancellationToken);

        if (sale is null)
            return CommandResultFactory.NotFound<SaleResult>("sale.not_found", "Sale not found.");

        var result = _mapper.Map<SaleResult>(sale);
        await _eventPublisher.PublishAsync(
            Events.SaleEventTopics.SaleCancelled,
            new Events.SaleCancelled(result),
            cancellationToken);
        return CommandResultFactory.Success(result);
    }
}
