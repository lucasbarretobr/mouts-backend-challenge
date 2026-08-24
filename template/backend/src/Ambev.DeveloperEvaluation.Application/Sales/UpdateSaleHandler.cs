using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Common.Messaging;
using Ambev.DeveloperEvaluation.Application.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public sealed class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, CommandResult<SaleResult>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _eventPublisher;

    public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IEventPublisher eventPublisher)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<CommandResult<SaleResult>> Handle(
        UpdateSaleCommand command,
        CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);

        if (sale is null)
            return CommandResultFactory.NotFound<SaleResult>("sale.not_found", "Sale not found.");

        try
        {
            sale.ChangeSaleNumber(command.SaleNumber);
            sale.SaleDate = command.SaleDate;
            sale.CustomerId = command.CustomerId;
            sale.CustomerName = command.CustomerName;
            sale.BranchId = command.BranchId;
            sale.BranchName = command.BranchName;
            sale.Items.Clear();

            foreach (var item in command.Items)
                sale.AddItem(_mapper.Map<SaleItem>(item));

            var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);
            var result = _mapper.Map<SaleResult>(updatedSale);
            await _eventPublisher.PublishAsync(
                Events.SaleEventTopics.SaleModified,
                new Events.SaleModified(result),
                cancellationToken);
            return CommandResultFactory.Success(result);
        }
        catch (DomainException exception)
        {
            return CommandResultFactory.Failure<SaleResult>(
                CommandResultFactory.Validation(
                    "sale.invalid",
                    "Sale is invalid: " + exception.Message));
        }
    }
}
