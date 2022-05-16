namespace CoffeeMachine.Code.Interfaces;

public interface IDrinkOrder
{
    public Products DrinkType { get; set; }
    public int Sugars { get; set; }
}