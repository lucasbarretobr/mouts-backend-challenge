using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleItemTests
{
    [Theory]
    [InlineData(1, 0)]
    [InlineData(3, 0)]
    [InlineData(4, 10)]
    [InlineData(9, 10)]
    [InlineData(10, 20)]
    [InlineData(20, 20)]
    public void Given_Quantity_When_CalculatingDiscount_Then_ShouldReturnExpectedPercentage(
        int quantity,
        decimal expectedDiscount)
    {
        var discount = SaleItem.CalculateDiscountPercentage(quantity);

        Assert.Equal(expectedDiscount, discount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(21)]
    public void Given_InvalidQuantity_When_CalculatingAmounts_Then_ShouldThrowDomainException(int quantity)
    {
        var item = new SaleItem
        {
            Quantity = quantity,
            UnitPrice = 10m
        };

        Assert.Throws<DomainException>(() => item.CalculateAmounts());
    }

    [Fact]
    public void Given_FourItems_When_CalculatingAmounts_Then_ShouldApplyTenPercentDiscount()
    {
        var item = new SaleItem
        {
            Quantity = 4,
            UnitPrice = 10m
        };

        item.CalculateAmounts();

        Assert.Equal(10m, item.DiscountPercentage);
        Assert.Equal(36m, item.TotalAmount);
    }

    [Fact]
    public void Given_TwentyItems_When_CalculatingAmounts_Then_ShouldApplyTwentyPercentDiscount()
    {
        var item = new SaleItem
        {
            Quantity = 20,
            UnitPrice = 10m
        };

        item.CalculateAmounts();

        Assert.Equal(20m, item.DiscountPercentage);
        Assert.Equal(160m, item.TotalAmount);
    }

    [Fact]
    public void Given_NegativeUnitPrice_When_CalculatingAmounts_Then_ShouldThrowDomainException()
    {
        var item = new SaleItem
        {
            Quantity = 1,
            UnitPrice = -1m
        };

        Assert.Throws<DomainException>(() => item.CalculateAmounts());
    }
}
