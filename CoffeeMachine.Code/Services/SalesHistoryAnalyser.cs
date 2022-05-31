using CoffeeMachine.Code.Models;

namespace CoffeeMachine.Code.Services;

public class SalesHistoryAnalyser
{
    public static List<SummaryReportLine> CreateSalesSummary(List<FulfilledDrinkOrder> drinkHistory, DateTime filterDate)
    {
        return drinkHistory
            .Where(d => d.TimePurchased.Date == filterDate.Date)
            .GroupBy(d => d.DrinkOrder.DrinkType)
            .Select(d => 
                new SummaryReportLine(d.Key, d.Count(), d.Sum(x => x.Price)))
            .OrderByDescending(x => x.Quantity)
            .ThenByDescending(x => x.Revenue)
            .ToList();
    }
    
    public static decimal CalculateTotalRevenue(List<SummaryReportLine> salesSummary) => salesSummary.Sum(x => x.Revenue);
}