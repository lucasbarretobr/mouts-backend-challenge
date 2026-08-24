using Ambev.DeveloperEvaluation.Application.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public record GetSaleQuery(Guid Id) : IRequest<CommandResult<SaleResult>>;
