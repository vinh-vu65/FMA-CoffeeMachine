namespace CoffeeMachine.Code.Interfaces;

public interface IDrinksCatalog
{ 
    CatalogRecord[] Catalog { get; }
    CatalogRecord QueryCatalog(IDrinkOrder drinkRequested);
}