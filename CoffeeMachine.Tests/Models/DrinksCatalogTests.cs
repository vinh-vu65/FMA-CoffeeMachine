using CoffeeMachine.Code;
using CoffeeMachine.Code.Models;
using Xunit;

namespace CoffeeMachine.Tests.Models;

public class DrinksCatalogTests
{
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
        var result = _sut.QueryCatalog(drinkType);
        
        Assert.Equal(expectedCode, result.DrinkCode);
    }
    
    [Theory]
    [InlineData(DrinkType.Coffee, 5.5)]
    [InlineData(DrinkType.HotChocolate, 6)]
    [InlineData(DrinkType.Tea, 4)]

    public void QueryCatalog_ShouldReturnCatalogRecordWithMatchingPrice_WhenDrinkTypesMatchInOrderAndCatalog(DrinkType drinkType, double expectedPrice)
    {
        var result = _sut.QueryCatalog(drinkType);
        
        Assert.Equal(expectedPrice, result.Price);
    }
}