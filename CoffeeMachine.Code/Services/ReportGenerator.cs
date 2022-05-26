using System.Text;
using CoffeeMachine.Code.Models;

namespace CoffeeMachine.Code.Services;

public class ReportGenerator : IReportGenerator
{
    public string GenerateHistoryReport(List<(DrinkOrder, DateTime, decimal)> drinkHistory, DateTime filterDate)
    {
        var desiredStringLength = 34;
        var output = new StringBuilder("\t\t\t-- Drink History --\n");
        output.Append("Drink\t\t\t\t\tTime");
        foreach (var (drinkOrder, dateTime, _) in drinkHistory.Where(d => d.Item2.Date == filterDate.Date))
        {
            output.Append("\n" + AddSpaces(drinkOrder.ToString(), desiredStringLength)); 
            output.Append("\t" + dateTime.ToString("dddd, dd MMMM yyyy hh:mm tt"));
        }

        return output.ToString();
    }

    public string GenerateSummaryReport(List<(DrinkOrder, DateTime, decimal)> drinkHistory, DateTime filterDate)
    {
        var salesSummary = CreateSalesSummary(drinkHistory, filterDate);
        var desiredStringLength = 15;
        var totalRevenue = CalculateTotalRevenue(salesSummary);
        
        var output = new StringBuilder("\t  -- Sales Summary --\n");
        output.Append("Drink\t\tQuantity\tRevenue\n");
        foreach (var (drinkType, quantity, revenue) in salesSummary)
        {
            output.Append(AddSpaces(drinkType.ToString(), desiredStringLength) + "\t");
            output.Append(quantity + "\t\t");
            output.Append(revenue + "\n");
        }

        output.Append($"\n\t\tTotal Revenue:  {totalRevenue}");
        return output.ToString();
    }

    public List<SummaryReportLine> CreateSalesSummary(List<(DrinkOrder, DateTime, decimal)> drinkHistory, DateTime filterDate)
    {
        return drinkHistory
            .Where(d => d.Item2.Date == filterDate.Date)
            .GroupBy(d => d.Item1)
            .Select(d => 
                new SummaryReportLine(d.Key.DrinkType, d.Count(), d.Sum(x => x.Item3)))
            .OrderByDescending(x => x.Quantity)
            .ThenByDescending(x => x.Revenue)
            .ToList();
    }

    public decimal CalculateTotalRevenue(List<SummaryReportLine> salesSummary) => salesSummary.Sum(x => x.Revenue);
    
    private string AddSpaces(string s, int length)
    {
        var difference = length - s.Length;
        return s + new string(' ', difference);
    }
}