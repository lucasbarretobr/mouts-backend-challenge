using Ambev.DeveloperEvaluation.Application.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public record CancelSaleCommand(Guid Id) : IRequest<CommandResult<SaleResult>>;
