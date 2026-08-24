using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    public string SaleNumber { get; private set; } = string.Empty;

    public DateTime SaleDate { get; set; }

    public Guid CustomerId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public Guid BranchId { get; set; }

    public string BranchName { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }

    public bool IsCancelled { get; set; }

    public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();

    public Sale()
    {
        SaleDate = DateTime.UtcNow;
    }

    public void AddItem(SaleItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (IsCancelled)
            throw new DomainException("Cannot add items to a cancelled sale.");

        item.CalculateAmounts();
        Items.Add(item);
        RecalculateTotalAmount();
    }

    public void ChangeSaleNumber(string saleNumber)
    {
        SaleNumber = saleNumber;
    }

    public void RecalculateTotalAmount()
    {
        TotalAmount = Math.Round(
            Items.Where(item => !item.IsCancelled).Sum(item => item.TotalAmount),
            2,
            MidpointRounding.AwayFromZero);
    }

    public void Cancel()
    {
        IsCancelled = true;
    }

    public void CancelItem(Guid itemId)
    {
        var item = Items.FirstOrDefault(saleItem => saleItem.Id == itemId)
            ?? throw new KeyNotFoundException($"Sale item with ID {itemId} not found");

        item.IsCancelled = true;
        RecalculateTotalAmount();
    }
}

