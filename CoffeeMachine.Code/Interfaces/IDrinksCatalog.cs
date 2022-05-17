using CoffeeMachine.Code.Models;

namespace CoffeeMachine.Code.Interfaces;

public interface IDrinksCatalog
{ 
    CatalogRecord QueryCatalog(DrinkType drinkType);
}