using CoffeeMachine.Code.Interfaces;

namespace CoffeeMachine.Code.Models;

public class DrinksCatalog : IDrinksCatalog
{
    public const double CoffeePrice = 5.5;
    public const double HotChocPrice = 6;
    public const double TeaPrice = 4;
    public const string CoffeeDrinkCode = "C";
    public const string HotChocDrinkCode = "H";
    public const string TeaDrinkCode = "T";
    
    public CatalogRecord[] Catalog { get; }

    public DrinksCatalog()
    {
        Catalog = new CatalogRecord[]
        {
            new(DrinkType.Coffee, CoffeeDrinkCode, CoffeePrice),
            new(DrinkType.HotChocolate, HotChocDrinkCode, HotChocPrice),
            new(DrinkType.Tea, TeaDrinkCode, TeaPrice)
        };
    }

    public CatalogRecord QueryCatalog(DrinkType drinkType)
    {
        return Catalog
            .First(d => d.DrinkType == drinkType);
    }
}