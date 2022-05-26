using CoffeeMachine.Code.Models;

namespace CoffeeMachine.Code.Services;

public interface IBeverageQuantityChecker
{
    bool IsEmpty(DrinkType drinkType);
}