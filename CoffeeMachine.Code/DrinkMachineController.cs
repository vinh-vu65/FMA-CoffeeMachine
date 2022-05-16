using CoffeeMachine.Code.Interfaces;

namespace CoffeeMachine.Code;

public class DrinkMachineController
{
    public string DrinkMakerProtocol { get; private set; }
    public CatalogRecord DrinkInfo { get; private set; }
    private readonly IDrinksCatalog _catalog;
    private readonly IProtocolBuilder _protocolBuilder;

    public DrinkMachineController(IDrinksCatalog catalog, IProtocolBuilder protocolBuilder)
    {
        _catalog = catalog;
        _protocolBuilder = protocolBuilder;
    }
    
    public void SendDrinkMakerProtocol(IDrink drinkRequested, double moneyInserted)
    {
        MatchDrinkInfo(drinkRequested);
        if (!IsSufficient(moneyInserted))
        {
            var message = $"Please insert another ${DrinkInfo.Price - moneyInserted} to receive your drink";
            DrinkMakerProtocol = _protocolBuilder.BuildMessage(message);
            return;
        }
        
        DrinkMakerProtocol = _protocolBuilder.BuildDrink();
    }

    public void MatchDrinkInfo(IDrink drinkRequested)
    {
        DrinkInfo = _catalog.QueryCatalog(drinkRequested);
    }
    
    private bool IsSufficient(double moneyInserted)
    {
        return moneyInserted > DrinkInfo.Price;
    }
}