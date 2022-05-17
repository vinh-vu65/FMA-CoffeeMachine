using CoffeeMachine.Code.Interfaces;
using CoffeeMachine.Code.Models;

namespace CoffeeMachine.Code.Controller;

public class DrinkMachineController
{
    public CatalogRecord DrinkInfo { get; private set; }
    private readonly IDrinksCatalog _catalog;
    private readonly IProtocolBuilder _protocolBuilder;

    public DrinkMachineController(IDrinksCatalog catalog, IProtocolBuilder protocolBuilder)
    {
        _catalog = catalog;
        _protocolBuilder = protocolBuilder;
    }
    
    public string SendDrinkMakerProtocol(DrinkOrder drinkRequested, double moneyInserted)
    {
        MatchDrinkInfo(drinkRequested);
        
        if (!IsSufficient(moneyInserted))
        {
            var message = $"Please insert another ${DrinkInfo.Price - moneyInserted} to receive your drink";
            return _protocolBuilder.BuildMessage(message);
        }
        
        return _protocolBuilder.BuildDrink(DrinkInfo.DrinkCode, drinkRequested.Sugars);
    }
    
    private void MatchDrinkInfo(DrinkOrder drinkRequested)
    {
        DrinkInfo = _catalog.QueryCatalog(drinkRequested.DrinkType);
    }
    
    private bool IsSufficient(double moneyInserted)
    {
        return moneyInserted >= DrinkInfo.Price;
    }
}