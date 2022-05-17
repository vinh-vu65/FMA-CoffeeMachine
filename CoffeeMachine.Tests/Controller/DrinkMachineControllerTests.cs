using CoffeeMachine.Code;
using CoffeeMachine.Code.Controller;
using CoffeeMachine.Code.Interfaces;
using CoffeeMachine.Code.Models;
using NSubstitute;
using Xunit;

namespace CoffeeMachine.Tests.Controller;

public class DrinkMachineControllerTests
{
    private DrinkOrder _drinkOrder;
    private readonly DrinkMachineController _sut;
    private readonly IDrinksCatalog _catalog = Substitute.For<IDrinksCatalog>();
    private readonly IProtocolBuilder _protocolBuilder = Substitute.For<IProtocolBuilder>();

    public DrinkMachineControllerTests()
    {
        _sut = new DrinkMachineController(_catalog, _protocolBuilder);
    }
    
    [Fact]
    public void SendDrinkProtocol_ShouldSetDrinkInfo_WhenDrinkTypeMatchesCatalogRecord()
    {
        var sampleRecord = new CatalogRecord(DrinkType.Coffee, "A", 0);
        _catalog.QueryCatalog(Arg.Any<DrinkType>()).Returns(sampleRecord);
        _drinkOrder = new DrinkOrder(DrinkType.Coffee, 2);
        
        _sut.SendDrinkMakerProtocol(_drinkOrder, 10);

        Assert.Equal(sampleRecord, _sut.DrinkInfo);
    }
    
    [Fact]
    public void SendDrinkProtocol_ShouldCallQueryCatalogMethod()
    {
        var sampleRecord = new CatalogRecord(DrinkType.Coffee, "A", 0);
        _catalog.QueryCatalog(Arg.Any<DrinkType>()).Returns(sampleRecord);
        _drinkOrder = new DrinkOrder(DrinkType.Coffee, 2);
        
        _sut.SendDrinkMakerProtocol(_drinkOrder, 10);

        _catalog.Received(1).QueryCatalog(_drinkOrder.DrinkType);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(10)]
    [InlineData(0.1)]
    public void SendDrinkProtocol_ShouldCallProtocolBuildDrink_WhenSufficientMoneyIsInserted(double moneyInserted)
    {
        var sampleRecord = new CatalogRecord(DrinkType.Coffee, "A", 0);
        _catalog.QueryCatalog(Arg.Any<DrinkType>()).Returns(sampleRecord);
        _drinkOrder = new DrinkOrder(DrinkType.Coffee, 2);

        _sut.SendDrinkMakerProtocol(_drinkOrder, moneyInserted);
        
        _protocolBuilder.Received(1).BuildDrink("A", Arg.Any<int>());
        _protocolBuilder.Received(0).BuildMessage(Arg.Any<string>());
    }
    
    [Theory]
    [InlineData(1.0)]
    [InlineData(9.99)]
    [InlineData(0.1)]
    public void SendDrinkProtocol_ShouldCallProtocolBuildMessage_WhenInsufficientMoneyIsInserted(double moneyInserted)
    {
        var sampleRecord = new CatalogRecord(DrinkType.Coffee, "A", 10);
        _catalog.QueryCatalog(Arg.Any<DrinkType>()).Returns(sampleRecord);
        _drinkOrder = new DrinkOrder(DrinkType.Coffee, 2);

        _sut.SendDrinkMakerProtocol(_drinkOrder, moneyInserted);
        
        _protocolBuilder.Received(1).BuildMessage(Arg.Any<string>());
        _protocolBuilder.Received(0).BuildDrink("A", Arg.Any<int>());
    }
    
    [Theory]
    [InlineData(1.0, 9)]
    [InlineData(9.99, 0.01)]
    [InlineData(0.1, 9.9)]
    public void SendDrinkProtocol_ShouldIncludeRemainingMoneyRequired_WhenInsufficientMoneyIsInserted(double moneyInserted, double moneyDifference)
    {
        var sampleRecord = new CatalogRecord(DrinkType.Coffee, "A", 10);
        _catalog.QueryCatalog(Arg.Any<DrinkType>()).Returns(sampleRecord);
        var expectedMessage = $"{moneyDifference}";
        _protocolBuilder.BuildMessage(Arg.Any<string>()).Returns(expectedMessage);
        _drinkOrder = new DrinkOrder(DrinkType.Coffee, 2);

        var result = _sut.SendDrinkMakerProtocol(_drinkOrder, moneyInserted);
        
        Assert.Contains(expectedMessage, result);
    }
    
    // Integration tests:
    [Theory]
    [InlineData(DrinkType.Coffee, 2, "C:2:1")]
    [InlineData(DrinkType.HotChocolate, 0, "H:0:0")]
    [InlineData(DrinkType.Tea, 3, "T:2:1")]
    [InlineData(DrinkType.Coffee, 1, "C:1:1")]
    public void SendDrinkProtocol_ShouldReturnDrinkProtocol_WhenDrinkOrderIsGivenAndEnoughMoneyIsInserted(DrinkType drinkType, int sugar, string expected)
    {
        var drink = new DrinkOrder(drinkType, sugar);
        var catalog = new DrinksCatalog();
        var builder = new ProtocolBuilder();
        var sut = new DrinkMachineController(catalog, builder);

        var result = sut.SendDrinkMakerProtocol(drink, 10);
        
        Assert.Equal(expected, result);
    }
}