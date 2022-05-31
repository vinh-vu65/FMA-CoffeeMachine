using CoffeeMachine.Code.Models;
using CoffeeMachine.Code.Services;
using Xunit;

namespace CoffeeMachine.Tests.Services;

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
    public void BuildDrinkCommand_ShouldReturnStringStartingWithDrinkCode_WhenDrinkCodeIsGiven(string drinkCode)
    {
        var drinkOrder = new DrinkOrder(DrinkType.Coffee, 0, false);

        var result = _sut.BuildDrinkCommand(drinkCode, drinkOrder);
        
        Assert.StartsWith(drinkCode, result);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(1)]
    public void BuildDrinkCommand_ShouldReturnStringWithSugarQuantityAfterDrinkCode(int sugarQuantity)
    {
        var drinkCode = "A";
        var drinkOrder = new DrinkOrder(DrinkType.Coffee, sugarQuantity, false);
        
        var result = _sut.BuildDrinkCommand(drinkCode, drinkOrder);
        
        Assert.StartsWith($"{drinkCode}:{sugarQuantity}", result);
    }
    
    [Theory]
    [InlineData(3)]
    [InlineData(2)]
    [InlineData(99)]
    public void BuildDrinkCommand_ShouldAllowMaximumOfTwoSugars(int sugarQuantity)
    {
        var drinkCode = "A";
        var drinkOrder = new DrinkOrder(DrinkType.Coffee, sugarQuantity, false);

        var result = _sut.BuildDrinkCommand(drinkCode, drinkOrder);
        
        Assert.StartsWith($"{drinkCode}:2", result);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(1)]
    public void BuildDrinkCommand_ShouldReturnStringEndingWithOneForStirStick_WhenDrinkContainsSugar(int sugarQuantity)
    {
        var drinkCode = "A";
        var drinkOrder = new DrinkOrder(DrinkType.Coffee, sugarQuantity, false);

        var result = _sut.BuildDrinkCommand(drinkCode, drinkOrder);
        
        Assert.Equal($"{drinkCode}:{sugarQuantity}:1", result);
    }
    
    [Fact]
    public void BuildDrinkCommand_ShouldReturnStringEndingWithZeroForNoStirStick_WhenDrinkContainsNoSugar()
    {
        var drinkCode = "A";
        var sugarQuantity = 0;
        var drinkOrder = new DrinkOrder(DrinkType.Coffee, sugarQuantity, false);

        var result = _sut.BuildDrinkCommand(drinkCode, drinkOrder);
        
        Assert.Equal($"{drinkCode}:{sugarQuantity}:0", result);
    }
    
    [Theory]
    [InlineData("This is a message")]
    [InlineData("This should display on drink maker")]
    public void BuildMessageCommand_ShouldReturnStringWithAMessage_WhenMessageIsGiven(string message)
    {
        var messageToDisplay = message;
        
        var result = _sut.BuildMessageCommand(messageToDisplay);
        
        Assert.Equal($"M:{message}", result);
    }

    [Theory]
    [InlineData(DrinkType.Coffee)]
    [InlineData(DrinkType.Tea)]
    [InlineData(DrinkType.HotChocolate)]
    public void BuildDrinkCommand_ShouldAddhToDrinkCode_WhenGivenDrinkOrderIsExtraHot(DrinkType drinkType)
    {
        var drinkOrder = new DrinkOrder(drinkType, 0, true);
        var baseDrinkCode = "A";

        var result = _sut.BuildDrinkCommand(baseDrinkCode, drinkOrder);
        
        Assert.Equal($"Ah:0:0", result);
    }
}