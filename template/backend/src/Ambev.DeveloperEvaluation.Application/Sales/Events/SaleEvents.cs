using Ambev.DeveloperEvaluation.Application.Sales;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

public sealed record SaleCreated(SaleResult Sale);

public sealed record SaleModified(SaleResult Sale);

public sealed record SaleCancelled(SaleResult Sale);

public sealed record ItemCancelled(SaleResult Sale, Guid ItemId);
