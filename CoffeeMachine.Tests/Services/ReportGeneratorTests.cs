using System;
using System.Collections.Generic;
using CoffeeMachine.Code.Models;
using CoffeeMachine.Code.Services;
using Xunit;

namespace CoffeeMachine.Tests.Services;

public class ReportGeneratorTests
{
    private readonly ReportGenerator _sut;
    private readonly List<FulfilledDrinkOrder> _sampleHistory;
    private readonly DateTime _filterDate;
    private readonly DrinkOrder _coffee = new(DrinkType.Coffee, 2, false);
    private readonly DrinkOrder _tea = new(DrinkType.Tea, 2, false);
    private readonly DrinkOrder _hotChocolate = new(DrinkType.HotChocolate, 2, true);

    
    public ReportGeneratorTests()
    {
        _sut = new ReportGenerator();
        _sampleHistory = new List<FulfilledDrinkOrder>
        {
            new(_coffee, new DateTime(2022, 5, 31, 10, 30, 0), 3m),
            new(_tea, new DateTime(2022, 5, 31, 11, 30, 0), 3m),
            new(_hotChocolate, new DateTime(2022, 5, 31, 11, 35, 0), 3m)
        };
        _filterDate = new DateTime(2022, 5, 31);
    }
    
    [Fact]
    public void GenerateHistoryReport_ShouldIncludeAllDrinksSoldInThatDay_WhenGivenDrinkHistoryAndDate()
    {
        var result = _sut.GenerateHistoryReport(_sampleHistory, _filterDate);

        Assert.Contains("Coffee, 2 sugar, Regular", result);
        Assert.Contains("Tea, 2 sugar, Regular", result);
        Assert.Contains("HotChocolate, 2 sugar, Extra hot", result);
    }
    
    [Fact]
    public void GenerateHistoryReport_ShouldIncludeAllDrinkTimeStampsSoldInThatDay_WhenGivenDrinkHistoryAndDate()
    {
        var result = _sut.GenerateHistoryReport(_sampleHistory, _filterDate);

        Assert.Contains("Tuesday, 31 May 2022 10:30 AM", result);
        Assert.Contains("Tuesday, 31 May 2022 11:30 AM", result);
        Assert.Contains("Tuesday, 31 May 2022 11:35 AM", result);
    }

    [Fact]
    public void GenerateHistoryReport_ShouldNotIncludeDrinks_WhenDaySoldDoesNotMatchDayGiven()
    {
        var result = _sut.GenerateHistoryReport(_sampleHistory, DateTime.MinValue);
        
        Assert.DoesNotContain("Tuesday, 31 May 2022 10:30 AM", result);
        Assert.DoesNotContain("Tuesday, 31 May 2022 11:30 AM", result);
        Assert.DoesNotContain("Tuesday, 31 May 2022 11:35 AM", result);
    }
}