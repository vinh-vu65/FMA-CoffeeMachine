namespace CoffeeMachine.Code;

public record CatalogRecord(DrinkType DrinkType, string DrinkCode, double Price)
{
    public readonly DrinkType DrinkType = DrinkType;
    public readonly string DrinkCode = DrinkCode;
    public readonly double Price = Price;
}