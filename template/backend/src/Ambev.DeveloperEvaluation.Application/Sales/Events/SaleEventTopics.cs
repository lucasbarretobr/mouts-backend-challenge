namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

public static class SaleEventTopics
{
    public const string SaleCreated = "sales.created";
    public const string SaleModified = "sales.modified";
    public const string SaleCancelled = "sales.cancelled";
    public const string ItemCancelled = "sales.item-cancelled";
}
