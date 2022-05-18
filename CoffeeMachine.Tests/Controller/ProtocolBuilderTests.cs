using CoffeeMachine.Code.Services;
using Xunit;

namespace CoffeeMachine.Tests.Controller;

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
    public void BuildDrink_ShouldReturnStringStartingWithDrinkCode_WhenDrinkCodeIsGiven(string drinkCode)
    {
        var result = _sut.BuildDrinkCommand(drinkCode, 1);
        
        Assert.StartsWith(drinkCode, result);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(1)]
    public void BuildDrink_ShouldReturnStringWithSugarQuantityAfterDrinkCode(int sugarQuantity)
    {
        var drinkCode = "A";
        
        var result = _sut.BuildDrinkCommand(drinkCode, sugarQuantity);
        
        Assert.StartsWith($"{drinkCode}:{sugarQuantity}", result);
    }
    
    [Theory]
    [InlineData(3)]
    [InlineData(2)]
    [InlineData(99)]
    public void BuildDrink_ShouldAllowMaximumOfTwoSugars(int sugarQuantity)
    {
        var drinkCode = "A";
        
        var result = _sut.BuildDrinkCommand(drinkCode, sugarQuantity);
        
        Assert.StartsWith($"{drinkCode}:2", result);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(1)]
    public void BuildDrink_ShouldReturnStringEndingWithOne_WhenDrinkContainsSugar(int sugarQuantity)
    {
        var drinkCode = "A";
        
        var result = _sut.BuildDrinkCommand(drinkCode, sugarQuantity);
        
        Assert.Equal($"{drinkCode}:{sugarQuantity}:1", result);
    }
    
    [Fact]
    public void BuildDrink_ShouldReturnStringEndingWithZero_WhenDrinkContainsNoSugar()
    {
        var drinkCode = "A";
        var sugars = 0;
        
        var result = _sut.BuildDrinkCommand(drinkCode, sugars);
        
        Assert.Equal($"{drinkCode}:{sugars}:0", result);
    }
    
    [Theory]
    [InlineData("This is a message")]
    [InlineData("This should display on drink maker")]
    public void BuildMessage_ShouldReturnStringWithAMessage_WhenMessageIsGiven(string message)
    {
        var messageToDisplay = message;
        
        var result = _sut.BuildMessageCommand(messageToDisplay);
        
        Assert.Equal($"M:{message}", result);
    }
}