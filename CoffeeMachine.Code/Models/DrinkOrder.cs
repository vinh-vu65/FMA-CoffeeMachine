namespace CoffeeMachine.Code.Models;

public class DrinkOrder
{
    public DrinkType DrinkType { get; }
    public int Sugars { get; }
    public bool IsExtraHot { get; }

    public DrinkOrder(DrinkType drinkType, int sugars, bool isExtraHot)
    {
        DrinkType = drinkType;
        Sugars = sugars;
        IsExtraHot = isExtraHot;
    }
}