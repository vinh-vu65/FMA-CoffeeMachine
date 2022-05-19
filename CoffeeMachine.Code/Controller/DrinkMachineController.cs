using CoffeeMachine.Code.Models;
using CoffeeMachine.Code.Services;

namespace CoffeeMachine.Code.Controller;

public class DrinkMachineController
{
    private readonly IDrinksCatalog _catalog;
    private readonly IProtocolBuilder _protocolBuilder;

    public DrinkMachineController(IDrinksCatalog catalog, IProtocolBuilder protocolBuilder)
    {
        _catalog = catalog;
        _protocolBuilder = protocolBuilder;
    }
    
    public string CreateDrinkMakerCommand(DrinkOrder drinkRequested, decimal moneyInserted)
    {
        var drinkInfo = GetDrinkInfo(drinkRequested);
        
        if (moneyInserted < drinkInfo.Price)
        {
            var message = $"Please insert another ${drinkInfo.Price - moneyInserted} to receive your drink";
            return _protocolBuilder.BuildMessageCommand(message);
        }

        var drinkCode = drinkInfo.DrinkCode;
        if (drinkRequested.IsExtraHot)
        {
            drinkCode += "h";
        }
        return _protocolBuilder.BuildDrinkCommand(drinkCode, drinkRequested.Sugars);
    }
    
    private CatalogRecord GetDrinkInfo(DrinkOrder drinkRequested)
    {
        return _catalog.QueryCatalog(drinkRequested.DrinkType);
    }
}