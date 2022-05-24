using System;
using System.Collections.Generic;
using CoffeeMachine.Code.Models;
using CoffeeMachine.Code.Services;
using Xunit;

namespace CoffeeMachine.Tests.Services;

public class ReportGeneratorTests
{
    private ReportGenerator _sut;
    private List<(DrinkOrder, DateTime)> _sampleHistory;
    private readonly DrinkOrder _drink = new DrinkOrder(DrinkType.Coffee, 2, false);
    
    public ReportGeneratorTests()
    {
        _sut = new ReportGenerator();
        _sampleHistory = new List<(DrinkOrder, DateTime)>
        {
            (_drink, DateTime.Now),
            (_drink, DateTime.Now),
            (_drink, DateTime.Now)
        };
    }
    
    [Fact]
    public void GenerateHistory_ShouldReturnAllDrinksSoldInThatDay_WhenGivenDrinkHistoryAndDate()
    {
        var result = _sut.GenerateHistory(_sampleHistory, DateTime.Today);

        Assert.Contains(_sampleHistory[0].Item1.ToString(), result);
        Assert.Contains(_sampleHistory[1].Item1.ToString(), result);
        Assert.Contains(_sampleHistory[2].Item1.ToString(), result);
    }
    
    [Fact]
    public void GenerateHistory_ShouldReturnAllDrinkTimeStampsSoldInThatDay_WhenGivenDrinkHistoryAndDate()
    {
        var result = _sut.GenerateHistory(_sampleHistory, DateTime.Today);

        Assert.Contains(_sampleHistory[0].Item2.ToString("dddd, dd MMMM yyyy hh:mm tt"), result);
        Assert.Contains(_sampleHistory[1].Item2.ToString("dddd, dd MMMM yyyy hh:mm tt"), result);
        Assert.Contains(_sampleHistory[2].Item2.ToString("dddd, dd MMMM yyyy hh:mm tt"), result);
    }

    [Fact]
    public void GenerateHistory_ShouldNotIncludeDrinks_WhenDaySoldDoesNotMatchDayGiven()
    {
        var result = _sut.GenerateHistory(_sampleHistory, DateTime.MinValue);
        
        Assert.DoesNotContain(_sampleHistory[0].Item1.ToString(), result);
        Assert.DoesNotContain(_sampleHistory[1].Item1.ToString(), result);
        Assert.DoesNotContain(_sampleHistory[2].Item1.ToString(), result);
    }
}