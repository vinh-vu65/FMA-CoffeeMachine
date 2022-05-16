using CoffeeMachine.Code.Interfaces;

namespace CoffeeMachine.Code;

public class ProtocolBuilder : IProtocolBuilder
{ 
    public string BuildDrink(string drinkCode, int sugars)
    {
        var stirStick = sugars > 0 ? "1" : "0";
        return $"{drinkCode}:{sugars}:{stirStick}";
    }

    public string BuildMessage(string message)
    {
        return $"M:{message}";
    }
}