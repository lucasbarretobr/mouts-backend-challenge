using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    private const int MinimumQuantity = 1;
    private const int MaximumQuantity = 20;
    private const int DiscountThreshold = 4;
    private const int HigherDiscountThreshold = 10;

    public Guid SaleId { get; set; }

    public Sale Sale { get; set; } = null!;

    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal DiscountPercentage { get; set; }

    public decimal TotalAmount { get; set; }

    public bool IsCancelled { get; set; }

    public void CalculateAmounts()
    {
        ValidateQuantity(Quantity);

        if (UnitPrice < 0)
            throw new DomainException("Unit price cannot be negative.");

        DiscountPercentage = CalculateDiscountPercentage(Quantity);
        TotalAmount = Math.Round(
            Quantity * UnitPrice * (1 - DiscountPercentage / 100),
            2,
            MidpointRounding.AwayFromZero);
    }

    public void UpdateQuantity(int quantity)
    {
        ValidateQuantity(quantity);
        Quantity = quantity;
        CalculateAmounts();
    }

    public static decimal CalculateDiscountPercentage(int quantity)
    {
        ValidateQuantity(quantity);

        return quantity switch
        {
            >= HigherDiscountThreshold => 20m,
            >= DiscountThreshold => 10m,
            _ => 0m
        };
    }

    private static void ValidateQuantity(int quantity)
    {
        if (quantity is < MinimumQuantity or > MaximumQuantity)
            throw new DomainException("Item quantity must be between 1 and 20.");
    }
}
