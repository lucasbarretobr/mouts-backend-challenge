namespace Ambev.DeveloperEvaluation.Application.Sales;

public sealed class PagedSaleResult
{
    public IReadOnlyCollection<SaleResult> Items { get; init; } = [];
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public int TotalCount { get; init; }
}
