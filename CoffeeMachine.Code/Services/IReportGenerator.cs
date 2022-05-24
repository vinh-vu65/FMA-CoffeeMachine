using CoffeeMachine.Code.Models;

namespace CoffeeMachine.Code.Services;

public interface IReportGenerator
{
    string GenerateHistory(List<(DrinkOrder, DateTime)> drinkHistory, DateTime filterDate);
}