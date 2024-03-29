using CoffeeMachine.Code.Models;
using CoffeeMachine.Code.Services;

namespace CoffeeMachine.Code.Controller;

public class DrinkMachineManager
{
    public List<FulfilledDrinkOrder> DrinkHistory { get; }
    private readonly IDrinksCatalog _catalog;
    private readonly IProtocolBuilder _protocolBuilder;
    private readonly IDrinkMaker _drinkMaker;
    private readonly IReportGenerator _reportGenerator;
    private readonly IBeverageQuantityChecker _quantityChecker;
    private readonly IEmailNotifier _emailNotifier;

    public DrinkMachineManager(IDrinksCatalog catalog, IProtocolBuilder protocolBuilder, IDrinkMaker drinkMaker, 
        IReportGenerator reportGenerator, IBeverageQuantityChecker quantityChecker, IEmailNotifier emailNotifier)
    {
        _catalog = catalog;
        _protocolBuilder = protocolBuilder;
        _drinkMaker = drinkMaker;
        _reportGenerator = reportGenerator;
        _quantityChecker = quantityChecker;
        _emailNotifier = emailNotifier;
        DrinkHistory = new List<FulfilledDrinkOrder>();
    }
    
    public void ManageDrinkOrder(DrinkOrder drinkRequested, decimal moneyInserted)
    {
        if (drinkRequested.IsExtraHot && drinkRequested.DrinkType == DrinkType.OrangeJuice)
        {
            var message = "Selected drink cannot be made extra hot, please enter a new drink order.";
            _drinkMaker.SendCommand(_protocolBuilder.BuildMessageCommand(message));
            return;
        }
        
        if (_quantityChecker.IsEmpty(drinkRequested.DrinkType))
        {
            var message = "Sorry, the drink you have ordered is currently unavailable, an email has been sent to your" +
                          "office administrator to refill the drink machine, please enter a new drink order.";
            _drinkMaker.SendCommand(_protocolBuilder.BuildMessageCommand(message));
            _emailNotifier.NotifyMissingDrink(drinkRequested.DrinkType);
            return;
        }
        
        var drinkInfo = GetDrinkInfo(drinkRequested);
        
        if (moneyInserted < drinkInfo.Price)
        {
            var message = $"Please insert another ${drinkInfo.Price - moneyInserted} to receive your drink";
            _drinkMaker.SendCommand(_protocolBuilder.BuildMessageCommand(message));
            return;
        }

        DrinkHistory.Add(new FulfilledDrinkOrder(drinkRequested, DateTime.Now, drinkInfo.Price));
        _drinkMaker.SendCommand(_protocolBuilder.BuildDrinkCommand(drinkInfo.DrinkCode, drinkRequested));
    }
    
    private CatalogRecord GetDrinkInfo(DrinkOrder drinkRequested)
    {
        return _catalog.QueryCatalog(drinkRequested.DrinkType);
    }

    public void PrintSalesHistory(DateTime date)
    {
        Console.WriteLine(_reportGenerator.GenerateHistoryReport(DrinkHistory, date));
    }

    public void PrintSalesSummary(DateTime date)
    {
        Console.WriteLine(_reportGenerator.GenerateSummaryReport(DrinkHistory, date));
    }
}