using Ambev.DeveloperEvaluation.Application.Sales;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

public sealed class SaleRequest
{
    public string SaleNumber { get; init; } = string.Empty;
    public DateTime SaleDate { get; init; }
    public Guid CustomerId { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public Guid BranchId { get; init; }
    public string BranchName { get; init; } = string.Empty;
    public IReadOnlyCollection<SaleItemRequest> Items { get; init; } = [];
}
