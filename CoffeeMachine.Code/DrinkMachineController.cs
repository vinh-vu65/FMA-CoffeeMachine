using CoffeeMachine.Code.Interfaces;

namespace CoffeeMachine.Code;

public class DrinkMachineController
{
    public string DrinkMakerProtocol { get; private set; }
    public string DrinkCode { get; private set; }
    private readonly IDrinksCatalog _catalog;

    public DrinkMachineController(IDrinksCatalog catalog)
    {
        _catalog = catalog;
    }
    
    public void SetDrinkCode(IDrink drinkRequested)
    {
        DrinkCode = _catalog.QueryCatalog(drinkRequested).DrinkCode;
    }
    
    public void CreateDrinkMakerProtocol(IDrink drinkRequested)
    {
        var stirStick = drinkRequested.Sugars > 0 ? "1" : "0";
        DrinkMakerProtocol = $"{DrinkCode}:{drinkRequested.Sugars}:{stirStick}";
    }

    public void CreateDrinkMakerProtocol(string message)
    {
        DrinkMakerProtocol = $"M:{message}";
    }
}