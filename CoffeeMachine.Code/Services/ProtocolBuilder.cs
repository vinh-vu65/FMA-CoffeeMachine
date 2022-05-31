using CoffeeMachine.Code.Models;

namespace CoffeeMachine.Code.Services;

public class ProtocolBuilder : IProtocolBuilder
{ 
    private const int MaximumSugarOutput = 2;

    public string BuildDrinkCommand(string baseDrinkCode, DrinkOrder drinkOrder)
    {
        var drinkCode = drinkOrder.IsExtraHot ? $"{baseDrinkCode}h" : baseDrinkCode;
        var sugars = drinkOrder.Sugars > MaximumSugarOutput ? MaximumSugarOutput : drinkOrder.Sugars;
        var stirStick = sugars > 0 ? "1" : "0";
        return $"{drinkCode}:{sugars}:{stirStick}";
    }

    public string BuildMessageCommand(string message)
    {
        return $"M:{message}";
    }
}