namespace CoffeeMachine.Code;

public interface IDrinksCatalog
{ 
    Dictionary<Products, string> Catalog { get; set; }
    string MatchDrinkCode(IDrink drinkRequested);
}