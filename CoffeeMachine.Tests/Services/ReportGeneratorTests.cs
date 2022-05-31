using System;
using System.Collections.Generic;
using CoffeeMachine.Code.Models;
using CoffeeMachine.Code.Services;
using Xunit;

namespace CoffeeMachine.Tests.Services;

public class ReportGeneratorTests
{
    private ReportGenerator _sut;
    private List<FulfilledDrinkOrder> _sampleHistory;
    private readonly DrinkOrder _coffee = new(DrinkType.Coffee, 2, false);
    private readonly DrinkOrder _tea = new(DrinkType.Tea, 2, false);
    private readonly DrinkOrder _hotChocolate = new(DrinkType.HotChocolate, 2, false);

    
    public ReportGeneratorTests()
    {
        _sut = new ReportGenerator();
        _sampleHistory = new List<FulfilledDrinkOrder>
        {
            new(_coffee, DateTime.Now, 3m),
            new(_coffee, DateTime.Now, 3m),
            new(_coffee, DateTime.Now, 3m)
        };
    }
    
    [Fact]
    public void GenerateHistoryReport_ShouldIncludeAllDrinksSoldInThatDay_WhenGivenDrinkHistoryAndDate()
    {
        var result = _sut.GenerateHistoryReport(_sampleHistory, DateTime.Today);

        Assert.Contains(_sampleHistory[0].DrinkOrder.ToString(), result);
        Assert.Contains(_sampleHistory[1].DrinkOrder.ToString(), result);
        Assert.Contains(_sampleHistory[2].DrinkOrder.ToString(), result);
    }
    
    [Fact]
    public void GenerateHistoryReport_ShouldIncludeAllDrinkTimeStampsSoldInThatDay_WhenGivenDrinkHistoryAndDate()
    {
        var result = _sut.GenerateHistoryReport(_sampleHistory, DateTime.Today);

        Assert.Contains(_sampleHistory[0].TimePurchased.ToString("dddd, dd MMMM yyyy hh:mm tt"), result);
        Assert.Contains(_sampleHistory[1].TimePurchased.ToString("dddd, dd MMMM yyyy hh:mm tt"), result);
        Assert.Contains(_sampleHistory[2].TimePurchased.ToString("dddd, dd MMMM yyyy hh:mm tt"), result);
    }

    [Fact]
    public void GenerateHistoryReport_ShouldNotIncludeDrinks_WhenDaySoldDoesNotMatchDayGiven()
    {
        var result = _sut.GenerateHistoryReport(_sampleHistory, DateTime.MinValue);
        
        Assert.DoesNotContain(_sampleHistory[0].DrinkOrder.ToString(), result);
        Assert.DoesNotContain(_sampleHistory[1].DrinkOrder.ToString(), result);
        Assert.DoesNotContain(_sampleHistory[2].DrinkOrder.ToString(), result);
    }

    [Fact]
    public void GetSalesSummary_ShouldIncludeNumberOfEachDrinkTypeSold_WhenGivenDrinkHistoryAndDate()
    {
        var expectedCoffeeQuantity = 3;
        var expectedTeaQuantity = 1;
        _sampleHistory.Add(new FulfilledDrinkOrder(_tea, DateTime.Now, 10m));

        var result = _sut.CreateSalesSummary(_sampleHistory, DateTime.Today);
        
        Assert.Equal(expectedCoffeeQuantity, result[0].Quantity);
        Assert.Equal(expectedTeaQuantity, result[1].Quantity);
    }
    
    [Fact]
    public void GetSalesSummary_ShouldIncludeRevenueOfEachDrinkTypeSold_WhenGivenDrinkHistoryAndDate()
    {
        var expectedCoffeeRevenue = 9;
        var expectedHotChocRevenue = 10;
        _sampleHistory.Add(new FulfilledDrinkOrder(_hotChocolate, DateTime.Now, 10m));

        var result = _sut.CreateSalesSummary(_sampleHistory, DateTime.Today);
        
        Assert.Equal(expectedCoffeeRevenue, result[0].Revenue);
        Assert.Equal(expectedHotChocRevenue, result[1].Revenue);
    }

    [Fact]
    public void GetSalesSummary_ShouldReturnEmptyList_WhenNoSalesHaveBeenMadeOnGivenDate()
    {
        var result = _sut.CreateSalesSummary(_sampleHistory, DateTime.MinValue);
        
        Assert.Empty(result);
    }

    [Fact]
    public void CalculateTotalRevenue_ShouldReturnSumOfAllRevenue_WhenGivenSalesSummary()
    {
        _sampleHistory.Add(new FulfilledDrinkOrder(_hotChocolate, DateTime.Now, 10m));
        var salesSummary = _sut.CreateSalesSummary(_sampleHistory, DateTime.Today);
        var expectedTotalRevenue = 19;

        var result = _sut.CalculateTotalRevenue(salesSummary);
        
        Assert.Equal(expectedTotalRevenue, result);
    }
}