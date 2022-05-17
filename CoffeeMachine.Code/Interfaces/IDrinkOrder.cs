namespace CoffeeMachine.Code.Interfaces;

public interface IDrinkOrder
{
    public DrinkType DrinkType { get; }
    public int Sugars { get; }
}