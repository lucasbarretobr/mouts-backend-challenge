using Ambev.DeveloperEvaluation.Application.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public sealed class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, CommandResult>
{
    private readonly ISaleRepository _saleRepository;

    public DeleteSaleHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<CommandResult> Handle(
        DeleteSaleCommand command,
        CancellationToken cancellationToken)
    {
        var deleted = await _saleRepository.DeleteAsync(command.Id, cancellationToken);

        return deleted
            ? CommandResultFactory.Success()
            : CommandResultFactory.Failure(
                CommandResultFactory.NotFound("sale.not_found", "Sale not found."));
    }
}
