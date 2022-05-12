namespace CoffeeMachine.Code;

public interface IDrink
{
    public Products DrinkType { get; set; }
    public int Sugars { get; set; }
}