using CoffeeMachine.Code.Interfaces;

namespace CoffeeMachine.Code.Controller;

public class DrinkMachineController
{
    private CatalogRecord _drinkInfo;
    private readonly IDrinksCatalog _catalog;
    private readonly IProtocolBuilder _protocolBuilder;

    public DrinkMachineController(IDrinksCatalog catalog, IProtocolBuilder protocolBuilder)
    {
        _catalog = catalog;
        _protocolBuilder = protocolBuilder;
    }
    
    public string SendDrinkMakerProtocol(IDrink drinkRequested, double moneyInserted)
    {
        MatchDrinkInfo(drinkRequested);
        if (!IsSufficient(moneyInserted))
        {
            var message = $"Please insert another ${_drinkInfo.Price - moneyInserted} to receive your drink";
            return _protocolBuilder.BuildMessage(message);
        }
        
        return _protocolBuilder.BuildDrink(_drinkInfo.DrinkCode, drinkRequested.Sugars);
    }

    public void MatchDrinkInfo(IDrink drinkRequested)
    {
        _drinkInfo = _catalog.QueryCatalog(drinkRequested);
    }
    
    private bool IsSufficient(double moneyInserted)
    {
        return moneyInserted > _drinkInfo.Price;
    }
}