namespace CoffeeMachine.Code.Interfaces;

public interface IProtocolBuilder
{
    string BuildDrink(string drinkCode, int sugars);
    string BuildMessage(string message);
}