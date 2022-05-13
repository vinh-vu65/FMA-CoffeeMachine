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
        var sampleRecord = new CatalogRecord(Products.Coffee, drinkCode);
        _catalog.QueryCatalog(Arg.Any<IDrink>()).Returns(sampleRecord);
        _sut.SetDrinkCode(_drink);

        _sut.CreateDrinkMakerProtocol(_drink);
        
        Assert.StartsWith(_sut.DrinkCode, _sut.DrinkMakerProtocol);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(99)]
    [InlineData(1)]
    public void CreateDrinkMakerProtocol_ShouldUpdateDrinkMakerProtocolWithSugarAmount_WhenDrinkOrderIsGiven(int sugarQuantity)
    {
        var sampleRecord = new CatalogRecord(Products.Coffee, "A");
        _catalog.QueryCatalog(Arg.Any<IDrink>()).Returns(sampleRecord);
        _drink.Sugars = sugarQuantity;
        
        _sut.CreateDrinkMakerProtocol(_drink);
        
        Assert.StartsWith($"{_sut.DrinkCode}:{_drink.Sugars}", _sut.DrinkMakerProtocol);
    }
    
    [Theory]
    [InlineData(99)]
    [InlineData(1)]
    public void CreateDrinkMakerProtocol_ShouldUpdateDrinkMakerProtocolWithOneAtTheEnd_WhenDrinkOrderContainsSugar(int sugarQuantity)
    {
        var sampleRecord = new CatalogRecord(Products.Coffee, "A");
        _catalog.QueryCatalog(Arg.Any<IDrink>()).Returns(sampleRecord);
        _drink.Sugars = sugarQuantity;
        
        _sut.CreateDrinkMakerProtocol(_drink);
        
        Assert.Equal($"{_sut.DrinkCode}:{_drink.Sugars}:1", _sut.DrinkMakerProtocol);
    }
    
    [Fact]
    public void CreateDrinkMakerProtocol_ShouldUpdateDrinkMakerProtocolWithZeroAtTheEnd_WhenDrinkOrderContainsNoSugar()
    {
        var sampleRecord = new CatalogRecord(Products.Coffee, "A");
        _catalog.QueryCatalog(Arg.Any<IDrink>()).Returns(sampleRecord);
        _drink.Sugars = 0;
        
        _sut.CreateDrinkMakerProtocol(_drink);
        
        Assert.Equal($"{_sut.DrinkCode}:{_drink.Sugars}:0", _sut.DrinkMakerProtocol);
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
        var sampleRecord = new CatalogRecord(Products.Coffee, "A");
        _catalog.QueryCatalog(Arg.Any<IDrink>()).Returns(sampleRecord);
        
        _sut.SetDrinkCode(_drink);

        _catalog.Received(1).QueryCatalog(_drink);
    }
}