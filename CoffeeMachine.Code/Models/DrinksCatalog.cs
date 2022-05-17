using CoffeeMachine.Code.Interfaces;

namespace CoffeeMachine.Code.Models;

public class DrinksCatalog : IDrinksCatalog
{
    public const double CoffeePrice = 5.5;
    public const double HotChocPrice = 6;
    public const double TeaPrice = 4;
    public CatalogRecord[] Catalog { get; }

    public DrinksCatalog()
    {
        Catalog = new CatalogRecord[]
        {
            new(DrinkType.Coffee, "C", CoffeePrice),
            new(DrinkType.HotChocolate, "H", HotChocPrice),
            new(DrinkType.Tea, "T", TeaPrice)
        };
    }

    public CatalogRecord QueryCatalog(IDrinkOrder drinkRequested)
    {
        return Catalog
            .First(d => d.DrinkType == drinkRequested.DrinkType);
    }
}