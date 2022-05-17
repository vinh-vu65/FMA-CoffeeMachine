namespace CoffeeMachine.Code.Controller;

public interface IProtocolBuilder
{
    string BuildDrinkCommand(string drinkCode, int sugars);
    string BuildMessageCommand(string message);
}