using CoffeeMachine.Code.Interfaces;

namespace CoffeeMachine.Code;

public class ProtocolBuilder : IProtocolBuilder
{
    public string DrinkCode { get; set; } = null!;
    public int Sugars { get; set; }

    public string BuildDrink()
    {
        var stirStick = Sugars > 0 ? "1" : "0";
        return $"{DrinkCode}:{Sugars}:{stirStick}";
    }

    public string BuildMessage(string message)
    {
        return $"M:{message}";
    }
}