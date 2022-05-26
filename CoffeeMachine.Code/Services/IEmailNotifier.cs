using CoffeeMachine.Code.Models;

namespace CoffeeMachine.Code.Services;

public interface IEmailNotifier
{
    void NotifyMissingDrink(DrinkType drinkType);
}