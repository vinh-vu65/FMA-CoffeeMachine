using CoffeeMachine.Code.Controller;

namespace CoffeeMachine.Code.Services;

public class ProtocolBuilder : IProtocolBuilder
{ 
    private const int MaximumSugarOutput = 2;

    public string BuildDrinkCommand(string drinkCode, int sugars)
    {
        sugars = sugars > MaximumSugarOutput ? MaximumSugarOutput : sugars;
        var stirStick = sugars > 0 ? "1" : "0";
        return $"{drinkCode}:{sugars}:{stirStick}";
    }

    public string BuildMessageCommand(string message)
    {
        return $"M:{message}";
    }
}