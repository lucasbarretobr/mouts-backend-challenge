using Ambev.DeveloperEvaluation.Application.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public sealed class CreateSaleCommand : IRequest<CommandResult<SaleResult>>
{
    public string SaleNumber { get; init; } = string.Empty;
    public DateTime SaleDate { get; init; }
    public Guid CustomerId { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public Guid BranchId { get; init; }
    public string BranchName { get; init; } = string.Empty;
    public IReadOnlyCollection<SaleItemInput> Items { get; init; } = [];
}
