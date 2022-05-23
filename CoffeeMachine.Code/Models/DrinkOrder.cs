namespace CoffeeMachine.Code.Models;

public class DrinkOrder
{
    public DrinkType DrinkType { get; }
    public int Sugars { get; }

    public DrinkOrder(DrinkType drinkType, int sugars)
    {
        DrinkType = drinkType;
        Sugars = sugars;
    }
}