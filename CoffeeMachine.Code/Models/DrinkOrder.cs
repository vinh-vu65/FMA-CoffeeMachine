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

    public override string ToString()
    {
        var hot = IsExtraHot ? "Extra hot" : "Regular";
        return $"{DrinkType.ToString()}, {Sugars} sugar, {hot}";
    }
}