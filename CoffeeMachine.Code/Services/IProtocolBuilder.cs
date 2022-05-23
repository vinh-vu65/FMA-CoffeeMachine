namespace CoffeeMachine.Code.Services;

public interface IProtocolBuilder
{
    string BuildDrinkCommand(string drinkCode, int sugars);
    string BuildMessageCommand(string message);
}