namespace CoffeeMachine.Code;

public class DrinkMachineController
{
    public string DrinkMakerProtocol { get; set; }
    
    public void CreateDrinkMakerProtocol(IDrink drinkRequested)
    {
        var stirStick = drinkRequested.Sugars > 0 ? "1" : "0";
        DrinkMakerProtocol = $"{drinkRequested.DrinkCode}:{drinkRequested.Sugars}:{stirStick}";
    }

    public void CreateDrinkMakerProtocol(string message)
    {
        DrinkMakerProtocol = $"M:{message}";
    }
}