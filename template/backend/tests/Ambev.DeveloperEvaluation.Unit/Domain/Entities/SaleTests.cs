using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    [Fact]
    public void Given_Items_When_Added_Then_ShouldCalculateSaleTotal()
    {
        var sale = new Sale();

        sale.AddItem(new SaleItem { Quantity = 4, UnitPrice = 10m });
        sale.AddItem(new SaleItem { Quantity = 2, UnitPrice = 15m });

        Assert.Equal(66m, sale.TotalAmount);
    }

    [Fact]
    public void Given_CancelledItem_When_TotalIsRecalculated_Then_ShouldExcludeCancelledItem()
    {
        var sale = new Sale();
        var item = new SaleItem { Quantity = 4, UnitPrice = 10m };

        sale.AddItem(item);
        sale.AddItem(new SaleItem { Quantity = 2, UnitPrice = 15m });
        item.IsCancelled = true;
        sale.RecalculateTotalAmount();

        Assert.Equal(30m, sale.TotalAmount);
    }

    [Fact]
    public void Given_CancelledSale_When_AddingItem_Then_ShouldThrowDomainException()
    {
        var sale = new Sale();
        sale.Cancel();

        Assert.Throws<DomainException>(() => sale.AddItem(new SaleItem
        {
            Quantity = 1,
            UnitPrice = 10m
        }));
    }
}
