namespace CoffeeMachine.Code.Interfaces;

public interface IDrinksCatalog
{ 
    Dictionary<Products, string> Catalog { get; set; }
    string MatchDrinkCode(IDrink drinkRequested);
}