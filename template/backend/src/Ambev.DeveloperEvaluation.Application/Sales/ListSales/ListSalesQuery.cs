using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public record ListSalesQuery(int Page = 1, int PageSize = 10) : IRequest<PagedSaleResult>;
