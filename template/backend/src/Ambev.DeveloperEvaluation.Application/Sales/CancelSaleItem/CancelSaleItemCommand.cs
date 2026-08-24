using Ambev.DeveloperEvaluation.Application.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public record CancelSaleItemCommand(Guid SaleId, Guid ItemId) : IRequest<CommandResult<SaleResult>>;
