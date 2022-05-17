using CoffeeMachine.Code.Interfaces;

namespace CoffeeMachine.Code;

public class DrinkOrder : IDrinkOrder
{
    public DrinkType DrinkType { get; }
    public int Sugars { get; }

    public DrinkOrder(DrinkType drinkType, int sugars)
    {
        DrinkType = drinkType;
        Sugars = sugars;
    }
}