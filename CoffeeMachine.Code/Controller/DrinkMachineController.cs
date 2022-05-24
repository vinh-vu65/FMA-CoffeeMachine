using CoffeeMachine.Code.Models;
using CoffeeMachine.Code.Services;

namespace CoffeeMachine.Code.Controller;

public class DrinkMachineController
{
    public List<(DrinkOrder, DateTime)> DrinkHistory { get; }
    private readonly IDrinksCatalog _catalog;
    private readonly IProtocolBuilder _protocolBuilder;
    private readonly IDrinkMaker _drinkMaker;

    public DrinkMachineController(IDrinksCatalog catalog, IProtocolBuilder protocolBuilder, IDrinkMaker drinkMaker)
    {
        _catalog = catalog;
        _protocolBuilder = protocolBuilder;
        _drinkMaker = drinkMaker;
        DrinkHistory = new List<(DrinkOrder, DateTime)>();
    }
    
    public void ManageDrinkOrder(DrinkOrder drinkRequested, decimal moneyInserted)
    {
        var drinkInfo = GetDrinkInfo(drinkRequested);

        if (drinkRequested.IsExtraHot && drinkInfo.DrinkType == DrinkType.OrangeJuice)
        {
            var message = "Selected drink cannot be made extra hot, please enter a new drink order.";
            _drinkMaker.SendCommand(_protocolBuilder.BuildMessageCommand(message));
            return;
        }
        
        if (moneyInserted < drinkInfo.Price)
        {
            var message = $"Please insert another ${drinkInfo.Price - moneyInserted} to receive your drink";
            _drinkMaker.SendCommand(_protocolBuilder.BuildMessageCommand(message));
            return;
        }

        DrinkHistory.Add((drinkRequested, DateTime.Now));
        _drinkMaker.SendCommand(_protocolBuilder.BuildDrinkCommand(drinkInfo.DrinkCode, drinkRequested));
    }
    
    private CatalogRecord GetDrinkInfo(DrinkOrder drinkRequested)
    {
        return _catalog.QueryCatalog(drinkRequested.DrinkType);
    }
}