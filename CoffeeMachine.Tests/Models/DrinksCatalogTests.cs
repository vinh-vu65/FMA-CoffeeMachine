using CoffeeMachine.Code;
using CoffeeMachine.Code.Interfaces;
using CoffeeMachine.Code.Models;
using NSubstitute;
using Xunit;

namespace CoffeeMachine.Tests.Models;

public class DrinksCatalogTests
{
    private readonly IDrinkOrder _drinkOrder = Substitute.For<IDrinkOrder>();
    private readonly DrinksCatalog _sut;

    public DrinksCatalogTests()
    {
        _sut = new DrinksCatalog();
    }
    
    [Theory]
    [InlineData(Products.Coffee, "C")]
    [InlineData(Products.HotChocolate, "H")]
    [InlineData(Products.Tea, "T")]

    public void QueryCatalog_ShouldReturnCatalogRecordWithMatchingDrinkCode_WhenDrinkTypesMatchInOrderAndCatalog(Products drinkType, string expectedCode)
    {
        _drinkOrder.DrinkType = drinkType;
        
        var result = _sut.QueryCatalog(_drinkOrder);
        
        Assert.Equal(expectedCode, result.DrinkCode);
    }
    
    [Theory]
    [InlineData(Products.Coffee, Constants.CoffeePrice)]
    [InlineData(Products.HotChocolate, Constants.HotChocPrice)]
    [InlineData(Products.Tea, Constants.TeaPrice)]

    public void QueryCatalog_ShouldReturnCatalogRecordWithMatchingPrice_WhenDrinkTypesMatchInOrderAndCatalog(Products drinkType, double expectedPrice)
    {
        _drinkOrder.DrinkType = drinkType;
        
        var result = _sut.QueryCatalog(_drinkOrder);
        
        Assert.Equal(expectedPrice, result.Price);
    }
}