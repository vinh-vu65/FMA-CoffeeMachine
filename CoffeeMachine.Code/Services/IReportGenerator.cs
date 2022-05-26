using CoffeeMachine.Code.Models;

namespace CoffeeMachine.Code.Services;

public interface IReportGenerator
{
    string GenerateHistoryReport(List<(DrinkOrder, DateTime, decimal)> drinkHistory, DateTime filterDate);
    string GenerateSummaryReport(List<(DrinkOrder, DateTime, decimal)> drinkHistory, DateTime filterDate);
}