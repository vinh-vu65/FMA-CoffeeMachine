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
    private readonly IDrinkMaker _drinkMaker = Substitute.For<IDrinkMaker>();

    public DrinkMachineControllerTests()
    {
        _sut = new DrinkMachineController(_catalog, _protocolBuilder, _drinkMaker);
        _drinkOrder = new DrinkOrder(DrinkType.Coffee, 2, false);
    }

    [Fact]
    public void CreateDrinkMakerCommand_ShouldCallQueryCatalogMethod()
    {
        var sampleRecord = new CatalogRecord(DrinkType.Coffee, "A", 0m);
        _catalog.QueryCatalog(Arg.Any<DrinkType>()).Returns(sampleRecord);
        
        _sut.ManageDrinkOrder(_drinkOrder, 10m);

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

        _sut.ManageDrinkOrder(_drinkOrder, moneyInserted);
        
        _protocolBuilder.Received(1).BuildDrinkCommand("A", Arg.Any<DrinkOrder>());
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

        _sut.ManageDrinkOrder(_drinkOrder, moneyInserted);
        
        _protocolBuilder.Received(1).BuildMessageCommand(Arg.Any<string>());
        _protocolBuilder.Received(0).BuildDrinkCommand("A", Arg.Any<DrinkOrder>());
    }
    
    [Theory]
    [InlineData(1.0, 10, 9)]
    [InlineData(9.99, 10, 0.01)]
    [InlineData(0.1, 10, 9.9)]
    public void CreateDrinkMakerCommand_ShouldIncludeRemainingMoneyRequired_WhenInsufficientMoneyIsInserted(decimal moneyInserted, decimal price, decimal moneyDifference)
    {
        var sampleRecord = new CatalogRecord(DrinkType.Coffee, "A", price);
        _catalog.QueryCatalog(Arg.Any<DrinkType>()).Returns(sampleRecord);
        var expectedMessage = $"{moneyDifference}";
        _protocolBuilder.BuildMessageCommand(Arg.Any<string>()).Returns(expectedMessage);

        _sut.ManageDrinkOrder(_drinkOrder, moneyInserted);
        
        _drinkMaker.Received(1).SendCommand(expectedMessage);
    }

    // Integration tests:
    [Theory]
    [InlineData(DrinkType.Coffee, 2, "C:2:1")]
    [InlineData(DrinkType.HotChocolate, 0, "H:0:0")]
    [InlineData(DrinkType.Tea, 3, "T:2:1")]
    public void CreateDrinkMakerCommand_ShouldReturnDrinkProtocol_WhenDrinkOrderIsGivenAndEnoughMoneyIsInserted(DrinkType drinkType, int sugar, string expected)
    {
        var drink = new DrinkOrder(drinkType, sugar, false);
        var catalog = new DrinksCatalog();
        var builder = new ProtocolBuilder();
        var sut = new DrinkMachineController(catalog, builder, _drinkMaker);

        sut.ManageDrinkOrder(drink, 10m);
        
        _drinkMaker.Received(1).SendCommand(expected);
    }
    
    [Theory]
    [InlineData(DrinkType.Coffee, 2, "Ch:2:1")]
    [InlineData(DrinkType.HotChocolate, 0, "Hh:0:0")]
    [InlineData(DrinkType.Tea, 3, "Th:2:1")]
    public void CreateDrinkMakerCommand_ShouldReturnDrinkProtocolWithH_WhenDrinkOrderIsExtraHotAndEnoughMoneyIsInserted(DrinkType drinkType, int sugar, string expected)
    {
        var drink = new DrinkOrder(drinkType, sugar, true);
        var catalog = new DrinksCatalog();
        var builder = new ProtocolBuilder();
        var sut = new DrinkMachineController(catalog, builder, _drinkMaker);

        sut.ManageDrinkOrder(drink, 10m);
        
        _drinkMaker.Received(1).SendCommand(expected);
    }

    [Fact]
    public void CreateDrinkMakerCommand_ShouldDisplayMessageToUserAndNotMakeDrink_WhenOrangeJuiceIsOrderedExtraHot()
    {
        var drink = new DrinkOrder(DrinkType.OrangeJuice, 2, true);
        var catalog = new DrinksCatalog();
        var sut = new DrinkMachineController(catalog, _protocolBuilder, _drinkMaker);
        var message = "Can't make a hot orange juice";
        _protocolBuilder.BuildMessageCommand(Arg.Any<string>()).Returns(message);

        sut.ManageDrinkOrder(drink, 10m);

        _drinkMaker.Received(1).SendCommand(message);
    }
}