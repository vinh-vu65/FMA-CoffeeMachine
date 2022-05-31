using CoffeeMachine.Code.Models;

namespace CoffeeMachine.Code.Services;

public interface IProtocolBuilder
{
    string BuildDrinkCommand(string baseDrinkCode, DrinkOrder drinkOrder);
    string BuildMessageCommand(string message);
}