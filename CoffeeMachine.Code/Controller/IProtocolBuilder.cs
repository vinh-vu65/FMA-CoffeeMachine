using CoffeeMachine.Code.Models;

namespace CoffeeMachine.Code.Controller;

public interface IProtocolBuilder
{
    string BuildDrinkCommand(string baseDrinkCode, DrinkOrder drinkOrder);
    string BuildMessageCommand(string message);
}