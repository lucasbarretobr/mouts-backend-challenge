using Ambev.DeveloperEvaluation.Application.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public record DeleteSaleCommand(Guid Id) : IRequest<CommandResult>;
