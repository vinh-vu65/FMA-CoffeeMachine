using CoffeeMachine.Code.Models;

namespace CoffeeMachine.Code.Services;

public class DrinksCatalog : IDrinksCatalog
{
    private readonly CatalogRecord[] _catalog;

    public DrinksCatalog()
    {
        _catalog = new CatalogRecord[]
        {
            new(DrinkType.Coffee, "C", 5.5),
            new(DrinkType.HotChocolate, "H", 6),
            new(DrinkType.Tea, "T", 4)
        };
    }

    public CatalogRecord QueryCatalog(DrinkType drinkType)
    {
        return _catalog
            .First(d => d.DrinkType == drinkType);
    }
}