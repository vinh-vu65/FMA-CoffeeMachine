using CoffeeMachine.Code;
using CoffeeMachine.Code.Controller;
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

        _sut.SendDrinkMakerProtocol(_drink, moneyInserted);
        
        _protocolBuilder.Received(1).BuildDrink("A", Arg.Any<int>());
        _protocolBuilder.Received(0).BuildMessage(Arg.Any<string>());

    }
    
    [Theory]
    [InlineData(1.0)]
    [InlineData(9.99)]
    [InlineData(0.1)]
    public void SendDrinkProtocol_ShouldCallProtocolBuildMessage_WhenInsufficientMoneyIsInserted(double moneyInserted)
    {
        var sampleRecord = new CatalogRecord(Products.Coffee, "A", 10);
        _catalog.QueryCatalog(Arg.Any<IDrink>()).Returns(sampleRecord);

        _sut.SendDrinkMakerProtocol(_drink, moneyInserted);
        
        _protocolBuilder.Received(1).BuildMessage(Arg.Any<string>());
        _protocolBuilder.Received(0).BuildDrink("A", Arg.Any<int>());
    }
    
    [Theory]
    [InlineData(1.0)]
    [InlineData(9.99)]
    [InlineData(0.1)]
    public void SendDrinkProtocol_ShouldIncludeRemainingMoneyRequired_WhenInsufficientMoneyIsInserted(double moneyInserted)
    {
        var sampleRecord = new CatalogRecord(Products.Coffee, "A", 10);
        _catalog.QueryCatalog(Arg.Any<IDrink>()).Returns(sampleRecord);
        var expectedMessage = $"M:Please insert another {10 - moneyInserted} to receive your drink";
        _protocolBuilder.BuildMessage(Arg.Any<string>()).Returns(expectedMessage);

        var result = _sut.SendDrinkMakerProtocol(_drink, moneyInserted);
        
        Assert.Equal(expectedMessage, result);
    }
}