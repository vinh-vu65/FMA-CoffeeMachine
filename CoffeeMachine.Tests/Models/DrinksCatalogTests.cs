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
    [InlineData(DrinkType.Coffee, "C")]
    [InlineData(DrinkType.HotChocolate, "H")]
    [InlineData(DrinkType.Tea, "T")]

    public void QueryCatalog_ShouldReturnCatalogRecordWithMatchingDrinkCode_WhenDrinkTypesMatchInOrderAndCatalog(DrinkType drinkType, string expectedCode)
    {
        _drinkOrder.DrinkType = drinkType;
        
        var result = _sut.QueryCatalog(_drinkOrder);
        
        Assert.Equal(expectedCode, result.DrinkCode);
    }
    
    [Theory]
    [InlineData(DrinkType.Coffee, DrinksCatalog.CoffeePrice)]
    [InlineData(DrinkType.HotChocolate, DrinksCatalog.HotChocPrice)]
    [InlineData(DrinkType.Tea, DrinksCatalog.TeaPrice)]

    public void QueryCatalog_ShouldReturnCatalogRecordWithMatchingPrice_WhenDrinkTypesMatchInOrderAndCatalog(DrinkType drinkType, double expectedPrice)
    {
        _drinkOrder.DrinkType = drinkType;
        
        var result = _sut.QueryCatalog(_drinkOrder);
        
        Assert.Equal(expectedPrice, result.Price);
    }
}