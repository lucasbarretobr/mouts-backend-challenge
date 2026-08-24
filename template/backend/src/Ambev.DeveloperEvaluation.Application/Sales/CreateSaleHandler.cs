using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Common.Messaging;
using Ambev.DeveloperEvaluation.Application.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public sealed class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CommandResult<SaleResult>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _eventPublisher;

    public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IEventPublisher eventPublisher)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<CommandResult<SaleResult>> Handle(
        CreateSaleCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var sale = new Sale();
            sale.ChangeSaleNumber(command.SaleNumber);
            sale.SaleDate = command.SaleDate == default ? DateTime.UtcNow : command.SaleDate;
            sale.CustomerId = command.CustomerId;
            sale.CustomerName = command.CustomerName;
            sale.BranchId = command.BranchId;
            sale.BranchName = command.BranchName;

            foreach (var item in command.Items)
                sale.AddItem(_mapper.Map<SaleItem>(item));

            var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
            var result = _mapper.Map<SaleResult>(createdSale);
            await _eventPublisher.PublishAsync(
                Events.SaleEventTopics.SaleCreated,
                new Events.SaleCreated(result),
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
