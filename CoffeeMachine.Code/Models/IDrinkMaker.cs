namespace CoffeeMachine.Code.Models;

public interface IDrinkMaker
{
    void SendCommand(string command);
}