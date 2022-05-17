namespace CoffeeMachine.Code.Interfaces;

public interface IProtocolBuilder
{
    string BuildDrinkCommand(string drinkCode, int sugars);
    string BuildMessageCommand(string message);
}