using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Common.Messaging;
using Ambev.DeveloperEvaluation.Application.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public sealed class CancelSaleItemHandler : IRequestHandler<CancelSaleItemCommand, CommandResult<SaleResult>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _eventPublisher;

    public CancelSaleItemHandler(ISaleRepository saleRepository, IMapper mapper, IEventPublisher eventPublisher)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<CommandResult<SaleResult>> Handle(
        CancelSaleItemCommand command,
        CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.CancelItemAsync(
            command.SaleId,
            command.ItemId,
            cancellationToken);

        if (sale is null)
            return CommandResultFactory.NotFound<SaleResult>(
                "sale_item.not_found",
                "Sale or sale item not found.");

        var result = _mapper.Map<SaleResult>(sale);
        await _eventPublisher.PublishAsync(
            Events.SaleEventTopics.ItemCancelled,
            new Events.ItemCancelled(result, command.ItemId),
            cancellationToken);
        return CommandResultFactory.Success(result);
    }
}
