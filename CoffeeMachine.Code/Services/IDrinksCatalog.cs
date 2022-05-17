using CoffeeMachine.Code.Models;

namespace CoffeeMachine.Code.Services;

public interface IDrinksCatalog
{ 
    CatalogRecord QueryCatalog(DrinkType drinkType);
}