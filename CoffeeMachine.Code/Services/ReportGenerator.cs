using System.Text;
using CoffeeMachine.Code.Models;

namespace CoffeeMachine.Code.Services;

public class ReportGenerator : IReportGenerator
{
    public string GenerateHistory(List<(DrinkOrder, DateTime)> drinkHistory, DateTime filterDate)
    {
        var desiredStringLength = 34;
        var output = new StringBuilder("\t\t\t-- Drink History --\n");
        output.Append("Drink\t\t\t\t\tTime");
        foreach (var (drinkOrder, dateTime) in drinkHistory.Where(d => d.Item2.Date == filterDate.Date))
        {
            output.Append("\n" + AddSpaces(drinkOrder.ToString(), desiredStringLength)); 
            output.Append("\t" + dateTime.ToString("dddd, dd MMMM yyyy hh:mm tt"));
        }

        return output.ToString();
    }

    private string AddSpaces(string s, int length)
    {
        var difference = length - s.Length;
        return s + new string(' ', difference);
    }

    
}