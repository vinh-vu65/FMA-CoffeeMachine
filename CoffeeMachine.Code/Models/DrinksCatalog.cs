using CoffeeMachine.Code.Interfaces;

namespace CoffeeMachine.Code.Models;

public class DrinksCatalog : IDrinksCatalog
{
    private const double CoffeePrice = 5.5;
    private const double HotChocPrice = 6;
    private const double TeaPrice = 4;
    private const string CoffeeDrinkCode = "C";
    private const string HotChocDrinkCode = "H";
    private const string TeaDrinkCode = "T";
    private readonly CatalogRecord[] _catalog;

    public DrinksCatalog()
    {
        _catalog = new CatalogRecord[]
        {
            new(DrinkType.Coffee, CoffeeDrinkCode, CoffeePrice),
            new(DrinkType.HotChocolate, HotChocDrinkCode, HotChocPrice),
            new(DrinkType.Tea, TeaDrinkCode, TeaPrice)
        };
    }

    public CatalogRecord QueryCatalog(DrinkType drinkType)
    {
        return _catalog
            .First(d => d.DrinkType == drinkType);
    }
}