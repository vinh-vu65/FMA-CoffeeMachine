using System;
using System.Collections.Generic;
using CoffeeMachine.Code.Models;
using CoffeeMachine.Code.Services;
using Xunit;

namespace CoffeeMachine.Tests.Services;

public class SalesHistoryAnalyserTests
{
    private SalesHistoryAnalyser _sut;
    private List<FulfilledDrinkOrder> _sampleHistory;
    private readonly DrinkOrder _coffee = new(DrinkType.Coffee, 2, false);
    private readonly DrinkOrder _tea = new(DrinkType.Tea, 2, false);
    private readonly DrinkOrder _hotChocolate = new(DrinkType.HotChocolate, 2, false);

    public SalesHistoryAnalyserTests()
    {
        _sut = new SalesHistoryAnalyser();
        _sampleHistory = new List<FulfilledDrinkOrder>
        {
            new(_coffee, DateTime.Now, 3m),
            new(_coffee, DateTime.Now, 3m),
            new(_coffee, DateTime.Now, 3m)
        };
    }
    
    [Fact]
    public void GetSalesSummary_ShouldIncludeNumberOfEachDrinkTypeSold_WhenGivenDrinkHistoryAndDate()
    {
        var expectedCoffeeQuantity = 3;
        var expectedTeaQuantity = 1;
        _sampleHistory.Add(new FulfilledDrinkOrder(_tea, DateTime.Now, 10m));

        var result = SalesHistoryAnalyser.CreateSalesSummary(_sampleHistory, DateTime.Today);
        
        Assert.Equal(expectedCoffeeQuantity, result[0].Quantity);
        Assert.Equal(expectedTeaQuantity, result[1].Quantity);
    }
    
    [Fact]
    public void GetSalesSummary_ShouldIncludeRevenueOfEachDrinkTypeSold_WhenGivenDrinkHistoryAndDate()
    {
        var expectedCoffeeRevenue = 9;
        var expectedHotChocRevenue = 10;
        _sampleHistory.Add(new FulfilledDrinkOrder(_hotChocolate, DateTime.Now, 10m));

        var result = SalesHistoryAnalyser.CreateSalesSummary(_sampleHistory, DateTime.Today);
        
        Assert.Equal(expectedCoffeeRevenue, result[0].Revenue);
        Assert.Equal(expectedHotChocRevenue, result[1].Revenue);
    }

    [Fact]
    public void GetSalesSummary_ShouldReturnEmptyList_WhenNoSalesHaveBeenMadeOnGivenDate()
    {
        var result = SalesHistoryAnalyser.CreateSalesSummary(_sampleHistory, DateTime.MinValue);
        
        Assert.Empty(result);
    }

    [Fact]
    public void CalculateTotalRevenue_ShouldReturnSumOfAllRevenue_WhenGivenSalesSummary()
    {
        _sampleHistory.Add(new FulfilledDrinkOrder(_hotChocolate, DateTime.Now, 10m));
        var salesSummary = SalesHistoryAnalyser.CreateSalesSummary(_sampleHistory, DateTime.Today);
        var expectedTotalRevenue = 19;

        var result = SalesHistoryAnalyser.CalculateTotalRevenue(salesSummary);
        
        Assert.Equal(expectedTotalRevenue, result);
    }
}