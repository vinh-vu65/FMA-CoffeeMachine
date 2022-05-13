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

    public void MatchDrinkCode_ShouldReturnDrinkCode_WhenDrinkTypesMatchInOrderAndCatalog(Products drinkType, string expectedCode)
    {
        _drink.DrinkType = drinkType;
        
        var result = _sut.MatchDrinkCode(_drink);
        
        Assert.Equal(expectedCode, result);
    }
}