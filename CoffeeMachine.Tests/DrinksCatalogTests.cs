using CoffeeMachine.Code;
using CoffeeMachine.Code.Interfaces;
using NSubstitute;
using Xunit;

namespace CoffeeMachine.Tests;

public class DrinksCatalogTests
{
    private readonly IDrink _drink = Substitute.For<IDrink>();
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
        _drink.DrinkType = drinkType;
        
        var result = _sut.QueryCatalog(_drink);
        
        Assert.Equal(expectedCode, result.DrinkCode);
    }
    
    [Theory]
    [InlineData(Products.Coffee, 5.5m)]
    [InlineData(Products.HotChocolate, 6m)]
    [InlineData(Products.Tea, 4m)]

    public void QueryCatalog_ShouldReturnCatalogRecordWithMatchingPrice_WhenDrinkTypesMatchInOrderAndCatalog(Products drinkType, decimal expectedPrice)
    {
        _drink.DrinkType = drinkType;
        
        var result = _sut.QueryCatalog(_drink);
        
        Assert.Equal(expectedPrice, result.Price);
    }
}