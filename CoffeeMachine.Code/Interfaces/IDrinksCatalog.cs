namespace CoffeeMachine.Code.Interfaces;

public interface IDrinksCatalog
{ 
    CatalogRecord[] Catalog { get; set; }
    CatalogRecord QueryCatalog(IDrinkOrder drinkRequested);
}