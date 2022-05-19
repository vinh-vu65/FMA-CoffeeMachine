using CoffeeMachine.Code.Controller;
using CoffeeMachine.Code.Models;
using CoffeeMachine.Code.Services;
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
    public void CreateDrinkMakerCommand_ShouldCallQueryCatalogMethod()
    {
        var sampleRecord = new CatalogRecord(DrinkType.Coffee, "A", 0m);
        _catalog.QueryCatalog(Arg.Any<DrinkType>()).Returns(sampleRecord);
        _drinkOrder = new DrinkOrder(DrinkType.Coffee, 2, false);
        
        _sut.CreateDrinkMakerCommand(_drinkOrder, 10m);

        _catalog.Received(1).QueryCatalog(_drinkOrder.DrinkType);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(10)]
    [InlineData(0.1)]
    public void CreateDrinkMakerCommand_ShouldCallProtocolBuildDrink_WhenSufficientMoneyIsInserted(decimal moneyInserted)
    {
        var sampleRecord = new CatalogRecord(DrinkType.Coffee, "A", 0m);
        _catalog.QueryCatalog(Arg.Any<DrinkType>()).Returns(sampleRecord);
        _drinkOrder = new DrinkOrder(DrinkType.Coffee, 2, false);

        _sut.CreateDrinkMakerCommand(_drinkOrder, moneyInserted);
        
        _protocolBuilder.Received(1).BuildDrinkCommand("A", Arg.Any<int>());
        _protocolBuilder.Received(0).BuildMessageCommand(Arg.Any<string>());
    }
    
    [Theory]
    [InlineData(1.0)]
    [InlineData(9.99)]
    [InlineData(0.1)]
    public void CreateDrinkMakerCommand_ShouldCallProtocolBuildMessage_WhenInsufficientMoneyIsInserted(decimal moneyInserted)
    {
        var sampleRecord = new CatalogRecord(DrinkType.Coffee, "A", 10m);
        _catalog.QueryCatalog(Arg.Any<DrinkType>()).Returns(sampleRecord);
        _drinkOrder = new DrinkOrder(DrinkType.Coffee, 2, false);

        _sut.CreateDrinkMakerCommand(_drinkOrder, moneyInserted);
        
        _protocolBuilder.Received(1).BuildMessageCommand(Arg.Any<string>());
        _protocolBuilder.Received(0).BuildDrinkCommand("A", Arg.Any<int>());
    }
    
    [Theory]
    [InlineData(1.0, 9)]
    [InlineData(9.99, 0.01)]
    [InlineData(0.1, 9.9)]
    public void CreateDrinkMakerCommand_ShouldIncludeRemainingMoneyRequired_WhenInsufficientMoneyIsInserted(decimal moneyInserted, decimal moneyDifference)
    {
        var sampleRecord = new CatalogRecord(DrinkType.Coffee, "A", 10m);
        _catalog.QueryCatalog(Arg.Any<DrinkType>()).Returns(sampleRecord);
        var expectedMessage = $"{moneyDifference}";
        _protocolBuilder.BuildMessageCommand(Arg.Any<string>()).Returns(expectedMessage);
        _drinkOrder = new DrinkOrder(DrinkType.Coffee, 2, false);

        var result = _sut.CreateDrinkMakerCommand(_drinkOrder, moneyInserted);
        
        Assert.Contains(expectedMessage, result);
    }

    // Integration tests:
    [Theory]
    [InlineData(DrinkType.Coffee, 2, "C:2:1")]
    [InlineData(DrinkType.HotChocolate, 0, "H:0:0")]
    [InlineData(DrinkType.Tea, 3, "T:2:1")]
    [InlineData(DrinkType.Coffee, 1, "C:1:1")]
    public void CreateDrinkMakerCommand_ShouldReturnDrinkProtocol_WhenDrinkOrderIsGivenAndEnoughMoneyIsInserted(DrinkType drinkType, int sugar, string expected)
    {
        var drink = new DrinkOrder(drinkType, sugar, false);
        var catalog = new DrinksCatalog();
        var builder = new ProtocolBuilder();
        var sut = new DrinkMachineController(catalog, builder);

        var result = sut.CreateDrinkMakerCommand(drink, 10m);
        
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData(DrinkType.Coffee, "Ch:2:1")]
    [InlineData(DrinkType.Tea, "Th:2:1")]
    [InlineData(DrinkType.HotChocolate, "Hh:2:1")]
    public void CreateDrinkMakerCommand_ShouldAddhToDrinkCode_WhenDrinkOrderIsExtraHot(DrinkType drinkType, string expected)
    {
        var drink = new DrinkOrder(drinkType, 2, true);
        var catalog = new DrinksCatalog();
        var builder = new ProtocolBuilder();
        var sut = new DrinkMachineController(catalog, builder);

        var result = sut.CreateDrinkMakerCommand(drink, 10m);
        
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void CreateDrinkMakerCommand_ShouldDisplayMessageToUserAndNotMakeDrink_WhenOrangeJuiceIsOrderedExtraHot()
    {
        var drink = new DrinkOrder(DrinkType.OrangeJuice, 2, true);
        var catalog = new DrinksCatalog();
        var builder = new ProtocolBuilder();
        var sut = new DrinkMachineController(catalog, builder);

        var result = sut.CreateDrinkMakerCommand(drink, 10m);

        Assert.StartsWith("M:", result);
    }
}