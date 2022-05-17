using CoffeeMachine.Code.Interfaces;

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
    
    public void MatchDrinkInfo(IDrinkOrder drinkRequested)
    {
        DrinkInfo = _catalog.QueryCatalog(drinkRequested.DrinkType);
    }
    
    public string SendDrinkMakerProtocol(IDrinkOrder drinkRequested, double moneyInserted)
    {
        if (!IsSufficient(moneyInserted))
        {
            var message = $"Please insert another ${DrinkInfo.Price - moneyInserted} to receive your drink";
            return _protocolBuilder.BuildMessage(message);
        }
        
        return _protocolBuilder.BuildDrink(DrinkInfo.DrinkCode, drinkRequested.Sugars);
    }
    
    private bool IsSufficient(double moneyInserted)
    {
        return moneyInserted >= DrinkInfo.Price;
    }
}