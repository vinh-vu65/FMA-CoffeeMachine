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
}