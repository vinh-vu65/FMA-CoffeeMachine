using CoffeeMachine.Code;
using NSubstitute;
using Xunit;

namespace CoffeeMachine.Tests;

public class DrinkMachineControllerTests
{
    private readonly DrinkMachineController _sut;
    private readonly IDrink _drink = Substitute.For<IDrink>();

    public DrinkMachineControllerTests()
    {
        _sut = new DrinkMachineController();
    }
    
    [Fact]
    public void CreateDrinkMakerProtocol_ShouldUpdateDrinkMakerProtocolWithDrinkCode_WhenDrinkOrderIsGiven()
    {
        _drink.DrinkCode = "C";

        _sut.CreateDrinkMakerProtocol(_drink);
        
        Assert.StartsWith(_drink.DrinkCode, _sut.DrinkMakerProtocol);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(99)]
    [InlineData(1)]
    public void CreateDrinkMakerProtocol_ShouldUpdateDrinkMakerProtocolWithSugarAmount_WhenDrinkOrderIsGiven(int sugarQuantity)
    {
        _drink.DrinkCode = "A";
        _drink.Sugars = sugarQuantity;
        
        _sut.CreateDrinkMakerProtocol(_drink);
        
        Assert.StartsWith($"{_drink.DrinkCode}:{_drink.Sugars}", _sut.DrinkMakerProtocol);
    }
}