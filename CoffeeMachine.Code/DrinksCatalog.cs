using CoffeeMachine.Code.Interfaces;

namespace CoffeeMachine.Code;

public class DrinksCatalog : IDrinksCatalog
{
    public CatalogRecord[] Catalog { get; set; }

    public DrinksCatalog()
    {
        Catalog = new CatalogRecord[]
        {
            new(Products.Coffee, "C"),
            new(Products.HotChocolate, "H"),
            new(Products.Tea, "T")
        };
    }

    public CatalogRecord QueryCatalog(IDrink drinkRequested)
    {
        return Catalog
            .First(d => d.DrinkType == drinkRequested.DrinkType);
    }
}