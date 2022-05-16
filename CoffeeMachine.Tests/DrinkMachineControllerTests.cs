using CoffeeMachine.Code;
using CoffeeMachine.Code.Interfaces;
using NSubstitute;
using Xunit;

namespace CoffeeMachine.Tests;

public class DrinkMachineControllerTests
{
    private readonly DrinkMachineController _sut;
    private readonly IDrink _drink = Substitute.For<IDrink>();
    private readonly IDrinksCatalog _catalog = Substitute.For<IDrinksCatalog>();
    private readonly IProtocolBuilder _protocolBuilder = Substitute.For<IProtocolBuilder>();

    public DrinkMachineControllerTests()
    {
        _sut = new DrinkMachineController(_catalog, _protocolBuilder);
    }
    
    [Fact]
    public void MatchDrinkInfo_ShouldUseDrinkCatalogToMatchDrinkCode()
    {
        var sampleRecord = new CatalogRecord(Products.Coffee, "A", 0);
        _catalog.QueryCatalog(Arg.Any<IDrink>()).Returns(sampleRecord);
        
        _sut.MatchDrinkInfo(_drink);

        _catalog.Received(1).QueryCatalog(_drink);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(10)]
    [InlineData(0.1)]
    public void SendDrinkProtocol_ShouldCallProtocolBuildDrink_WhenSufficientMoneyIsInserted(double moneyInserted)
    {
        var sampleRecord = new CatalogRecord(Products.Coffee, "A", 0);
        _catalog.QueryCatalog(Arg.Any<IDrink>()).Returns(sampleRecord);
        _sut.MatchDrinkInfo(_drink);

        _sut.SendDrinkMakerProtocol(_drink, moneyInserted);
        
        _protocolBuilder.Received(1).BuildDrink();
    }
    
    [Theory]
    [InlineData(1.0)]
    [InlineData(9.99)]
    [InlineData(0.1)]
    public void SendDrinkProtocol_ShouldCallProtocolBuildMessage_WhenInsufficientMoneyIsInserted(double moneyInserted)
    {
        var sampleRecord = new CatalogRecord(Products.Coffee, "A", 10);
        _catalog.QueryCatalog(Arg.Any<IDrink>()).Returns(sampleRecord);
        _sut.MatchDrinkInfo(_drink);

        _sut.SendDrinkMakerProtocol(_drink, moneyInserted);
        
        _protocolBuilder.Received(1).BuildMessage(Arg.Any<string>());
    }
    
    [Theory]
    [InlineData(1.0)]
    [InlineData(10)]
    [InlineData(0.1)]
    public void SendDrinkProtocol_ShouldCreateDrinkProtocolWithSugarAndStick_WhenSufficientMoneyIsInsertedAndOrderContainsSugar(double moneyInserted)
    {
        var sampleRecord = new CatalogRecord(Products.Coffee, "A", 0);
        _catalog.QueryCatalog(Arg.Any<IDrink>()).Returns(sampleRecord);
        _drink.Sugars = 2;
        _sut.MatchDrinkInfo(_drink);

        _sut.SendDrinkMakerProtocol(_drink, moneyInserted);
        
        Assert.Equal("A:2:1", _sut.DrinkMakerProtocol);
    }
    
    [Theory]
    [InlineData(1.0)]
    [InlineData(10)]
    [InlineData(0.1)]
    public void SendDrinkProtocol_ShouldCreateDrinkProtocolWithNoSugarAndStick_WhenSufficientMoneyIsInsertedAndOrderContainsNoSugar(double moneyInserted)
    {
        var sampleRecord = new CatalogRecord(Products.Coffee, "A", 0);
        _catalog.QueryCatalog(Arg.Any<IDrink>()).Returns(sampleRecord);
        _drink.Sugars = 0;
        _sut.MatchDrinkInfo(_drink);

        _sut.SendDrinkMakerProtocol(_drink, moneyInserted);
        
        Assert.Equal("A:0:0", _sut.DrinkMakerProtocol);
    }
}