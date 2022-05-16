using CoffeeMachine.Code.Interfaces;

namespace CoffeeMachine.Code.Models;

public class DrinksCatalog : IDrinksCatalog
{
    public CatalogRecord[] Catalog { get; set; }

    public DrinksCatalog()
    {
        Catalog = new CatalogRecord[]
        {
            new(Products.Coffee, "C", Constants.CoffeePrice),
            new(Products.HotChocolate, "H", Constants.HotChocPrice),
            new(Products.Tea, "T", Constants.TeaPrice)
        };
    }

    public CatalogRecord QueryCatalog(IDrink drinkRequested)
    {
        return Catalog
            .First(d => d.DrinkType == drinkRequested.DrinkType);
    }
}