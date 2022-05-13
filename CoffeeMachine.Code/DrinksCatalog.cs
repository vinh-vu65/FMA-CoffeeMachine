using CoffeeMachine.Code.Interfaces;

namespace CoffeeMachine.Code;

public class DrinksCatalog : IDrinksCatalog
{
    public Dictionary<Products, string> Catalog { get; set; }

    public DrinksCatalog()
    {
        Catalog = new Dictionary<Products, string>
        {
            {Products.Coffee, "C"},
            {Products.HotChocolate, "H"},
            {Products.Tea, "T"}
        };
    }

    public string GetDrinkCode(IDrink drinkRequested)
    {
        return Catalog
            .Where(d => d.Key == drinkRequested.DrinkType)
            .Select(d => d.Value)
            .First();
    }
}