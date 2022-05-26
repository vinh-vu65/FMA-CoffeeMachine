using CoffeeMachine.Code.Models;
using CoffeeMachine.Code.Services;

namespace CoffeeMachine.Code.Controller;

public class DrinkMachineManager
{
    public List<(DrinkOrder, DateTime, decimal)> DrinkHistory { get; }
    private readonly IDrinksCatalog _catalog;
    private readonly IProtocolBuilder _protocolBuilder;
    private readonly IDrinkMaker _drinkMaker;
    private readonly IReportGenerator _reportGenerator;
    private readonly IBeverageQuantityChecker _quantityChecker;

    public DrinkMachineManager(IDrinksCatalog catalog, IProtocolBuilder protocolBuilder, IDrinkMaker drinkMaker, IReportGenerator reportGenerator, IBeverageQuantityChecker quantityChecker)
    {
        _catalog = catalog;
        _protocolBuilder = protocolBuilder;
        _drinkMaker = drinkMaker;
        _reportGenerator = reportGenerator;
        _quantityChecker = quantityChecker;
        DrinkHistory = new List<(DrinkOrder, DateTime, decimal)>();
    }
    
    public void ManageDrinkOrder(DrinkOrder drinkRequested, decimal moneyInserted)
    {
        if (_quantityChecker.IsEmpty(drinkRequested.DrinkType))
        {
            var message = "Sorry, the drink you have ordered is currently unavailable, an email has been sent to your" +
                          "office administrator to refill the drink machine, please enter a new drink order.";
            _drinkMaker.SendCommand(_protocolBuilder.BuildMessageCommand(message));
            return;
        }
        
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

        DrinkHistory.Add((drinkRequested, DateTime.Now, drinkInfo.Price));
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