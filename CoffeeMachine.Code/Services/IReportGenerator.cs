using CoffeeMachine.Code.Models;

namespace CoffeeMachine.Code.Services;

public interface IReportGenerator
{
    string GenerateHistoryReport(List<FulfilledDrinkOrder> drinkHistory, DateTime filterDate);
    string GenerateSummaryReport(List<FulfilledDrinkOrder> drinkHistory, DateTime filterDate);
}