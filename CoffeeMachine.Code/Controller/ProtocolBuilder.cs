using CoffeeMachine.Code.Interfaces;

namespace CoffeeMachine.Code.Controller;

public class ProtocolBuilder : IProtocolBuilder
{ 
    public const int MaximumSugarOutput = 2;

    public string BuildDrink(string drinkCode, int sugars)
    {
        sugars = sugars > MaximumSugarOutput ? MaximumSugarOutput : sugars;
        var stirStick = sugars > 0 ? "1" : "0";
        return $"{drinkCode}:{sugars}:{stirStick}";
    }

    public string BuildMessage(string message)
    {
        return $"M:{message}";
    }
}