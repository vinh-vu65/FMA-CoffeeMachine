using CoffeeMachine.Code;
using Xunit;

namespace CoffeeMachine.Tests;

public class ProtocolBuilderTests
{
    private ProtocolBuilder _sut;
    
    public ProtocolBuilderTests()
    {
        _sut = new ProtocolBuilder();
    }

    [Theory]
    [InlineData("A")]
    [InlineData(" ")]
    [InlineData("")]
    public void BuildDrink_ShouldReturnStringStartingWithDrinkCode_WhenDrinkCodeIsSet(string drinkCode)
    {
        _sut.DrinkCode = drinkCode;

        var result = _sut.BuildDrink();
        
        Assert.StartsWith(drinkCode, result);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(99)]
    [InlineData(1)]
    public void BuildDrink_ShouldReturnStringWithSugarQuantityAfterDrinkCode(int sugarQuantity)
    {
        _sut.DrinkCode = "A";
        _sut.Sugars = sugarQuantity;
        
        var result = _sut.BuildDrink();
        
        Assert.StartsWith($"{_sut.DrinkCode}:{sugarQuantity}", result);
    }

    [Theory]
    [InlineData(99)]
    [InlineData(1)]
    public void BuildDrink_ShouldReturnStringEndingWithOne_WhenDrinkContainsSugar(int sugarQuantity)
    {
        _sut.DrinkCode = "A";
        _sut.Sugars = sugarQuantity;
        
        var result = _sut.BuildDrink();
        
        Assert.Equal($"{_sut.DrinkCode}:{_sut.Sugars}:1", result);
    }
    
    [Fact]
    public void BuildDrink_ShouldReturnStringEndingWithOne_WhenDrinkContainsNoSugar()
    {
        _sut.DrinkCode = "A";
        _sut.Sugars = 0;
        
        var result = _sut.BuildDrink();
        
        Assert.Equal($"{_sut.DrinkCode}:{_sut.Sugars}:0", result);
    }
    
    [Theory]
    [InlineData("This is a message")]
    [InlineData("This should display on drink maker")]
    public void BuildMessage_ShouldReturnStringWithAMessage_WhenMessageIsGiven(string message)
    {
        var messageToDisplay = message;
        
        var result = _sut.BuildMessage(messageToDisplay);
        
        Assert.Equal($"M:{message}", result);
    }
}