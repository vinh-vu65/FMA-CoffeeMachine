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

    public DrinkMachineControllerTests()
    {
        _sut = new DrinkMachineController(_catalog);
    }
    
    [Theory]
    [InlineData("C")]
    [InlineData("test")]
    public void CreateDrinkMakerProtocol_ShouldUpdateDrinkMakerProtocolWithDrinkCode_WhenDrinkOrderIsGiven(string drinkCode)
    {
        var sampleRecord = new CatalogRecord(Products.Coffee, drinkCode, 0);
        _catalog.QueryCatalog(Arg.Any<IDrink>()).Returns(sampleRecord);
        _sut.GetCatalogRecord(_drink);

        _sut.CreateDrinkMakerProtocol(_drink);
        
        Assert.StartsWith(_sut.DrinkInfo.DrinkCode, _sut.DrinkMakerProtocol);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(99)]
    [InlineData(1)]
    public void CreateDrinkMakerProtocol_ShouldUpdateDrinkMakerProtocolWithSugarAmount_WhenDrinkOrderIsGiven(int sugarQuantity)
    {
        var sampleRecord = new CatalogRecord(Products.Coffee, "A", 0);
        _catalog.QueryCatalog(Arg.Any<IDrink>()).Returns(sampleRecord);
        _drink.Sugars = sugarQuantity;
        _sut.GetCatalogRecord(_drink);
        
        _sut.CreateDrinkMakerProtocol(_drink);
        
        Assert.StartsWith($"{_sut.DrinkInfo.DrinkCode}:{_drink.Sugars}", _sut.DrinkMakerProtocol);
    }
    
    [Theory]
    [InlineData(99)]
    [InlineData(1)]
    public void CreateDrinkMakerProtocol_ShouldUpdateDrinkMakerProtocolWithOneAtTheEnd_WhenDrinkOrderContainsSugar(int sugarQuantity)
    {
        var sampleRecord = new CatalogRecord(Products.Coffee, "A", 0);
        _catalog.QueryCatalog(Arg.Any<IDrink>()).Returns(sampleRecord);
        _drink.Sugars = sugarQuantity;
        _sut.GetCatalogRecord(_drink);

        _sut.CreateDrinkMakerProtocol(_drink);
        
        Assert.Equal($"{_sut.DrinkInfo.DrinkCode}:{_drink.Sugars}:1", _sut.DrinkMakerProtocol);
    }
    
    [Fact]
    public void CreateDrinkMakerProtocol_ShouldUpdateDrinkMakerProtocolWithZeroAtTheEnd_WhenDrinkOrderContainsNoSugar()
    {
        var sampleRecord = new CatalogRecord(Products.Coffee, "A", 0);
        _catalog.QueryCatalog(Arg.Any<IDrink>()).Returns(sampleRecord);
        _drink.Sugars = 0;
        _sut.GetCatalogRecord(_drink);

        _sut.CreateDrinkMakerProtocol(_drink);
        
        Assert.Equal($"{_sut.DrinkInfo.DrinkCode}:{_drink.Sugars}:0", _sut.DrinkMakerProtocol);
    }

    [Theory]
    [InlineData("This is a message")]
    [InlineData("This should display on drink maker")]
    public void CreateDrinkMakerProtocol_ShouldUpdateDrinkMakerProtocolWithAMessage_WhenMessageIsGiven(string message)
    {
        var messageToDisplay = message;
        
        _sut.CreateDrinkMakerProtocol(messageToDisplay);
        
        Assert.Equal($"M:{message}", _sut.DrinkMakerProtocol);
    }

    [Fact]
    public void SetDrinkCode_ShouldUseDrinkCatalogToMatchDrinkCode()
    {
        var sampleRecord = new CatalogRecord(Products.Coffee, "A", 0);
        _catalog.QueryCatalog(Arg.Any<IDrink>()).Returns(sampleRecord);
        
        _sut.GetCatalogRecord(_drink);

        _catalog.Received(1).QueryCatalog(_drink);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(10)]
    [InlineData(0.1)]
    public void HasSufficientMoney_ShouldReturnTrue_WhenInsertedMoneyIsGreaterThanDrinkPrice(double moneyInserted)
    {
        var sampleRecord = new CatalogRecord(Products.Coffee, "A", 0);
        _catalog.QueryCatalog(Arg.Any<IDrink>()).Returns(sampleRecord);
        _sut.GetCatalogRecord(_drink);

        var result = _sut.HasSufficientMoney(moneyInserted);
        
        Assert.True(result);
    }
    
    [Theory]
    [InlineData(1.0)]
    [InlineData(9.99)]
    [InlineData(0.1)]
    public void HasSufficientMoney_ShouldReturnFalse_WhenInsertedMoneyIsLessThanDrinkPrice(double moneyInserted)
    {
        var sampleRecord = new CatalogRecord(Products.Coffee, "A", 10);
        _catalog.QueryCatalog(Arg.Any<IDrink>()).Returns(sampleRecord);
        _sut.GetCatalogRecord(_drink);

        var result = _sut.HasSufficientMoney(moneyInserted);
        
        Assert.False(result);
    }
}