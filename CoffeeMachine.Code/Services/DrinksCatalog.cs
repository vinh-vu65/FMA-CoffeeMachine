using CoffeeMachine.Code.Models;

namespace CoffeeMachine.Code.Services;

public class DrinksCatalog : IDrinksCatalog
{
    private readonly CatalogRecord[] _catalog;

    public DrinksCatalog()
    {
        _catalog = new CatalogRecord[]
        {
            new(DrinkType.Coffee, "C", 5.5m),
            new(DrinkType.HotChocolate, "H", 6m),
            new(DrinkType.Tea, "T", 4m)
        };
    }

    public CatalogRecord QueryCatalog(DrinkType drinkType)
    {
        return _catalog
            .First(d => d.DrinkType == drinkType);
    }
}