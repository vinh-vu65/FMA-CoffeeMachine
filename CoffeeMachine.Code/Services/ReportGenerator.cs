using System.Text;
using CoffeeMachine.Code.Models;

namespace CoffeeMachine.Code.Services;

public class ReportGenerator : IReportGenerator
{
    public string GenerateHistoryReport(List<FulfilledDrinkOrder> drinkHistory, DateTime filterDate)
    {
        var desiredStringLength = 34;
        var output = new StringBuilder("\t\t\t-- Drink History --\n");
        output.Append("Drink\t\t\t\t\tTime");
        foreach (var (drinkOrder, dateTime, _) in drinkHistory.Where(d => d.TimePurchased.Date == filterDate.Date))
        {
            output.Append("\n" + drinkOrder.ToString().PadRight(desiredStringLength)); 
            output.Append("\t" + dateTime.ToString("dddd, dd MMMM yyyy hh:mm tt"));
        }

        return output.ToString();
    }

    public string GenerateSummaryReport(List<FulfilledDrinkOrder> drinkHistory, DateTime filterDate)
    {
        var salesSummary = SalesHistoryAnalyser.CreateSalesSummary(drinkHistory, filterDate);
        var desiredStringLength = 15;
        var totalRevenue = SalesHistoryAnalyser.CalculateTotalRevenue(salesSummary);
        
        var output = new StringBuilder("\t  -- Sales Summary --\n");
        output.Append("Drink\t\tQuantity\tRevenue\n");
        foreach (var (drinkType, quantity, revenue) in salesSummary)
        {
            output.Append(drinkType.ToString().PadRight(desiredStringLength) + "\t");
            output.Append(quantity + "\t\t");
            output.Append(revenue + "\n");
        }

        output.Append($"\n\t\tTotal Revenue:  {totalRevenue}");
        return output.ToString();
    }
}