namespace CoffeeMachine.Code.Interfaces;

public interface IProtocolBuilder
{
    string DrinkCode { get; set; }
    int Sugars { get; set; }
    
    string BuildDrink();
    string BuildMessage(string message);
}