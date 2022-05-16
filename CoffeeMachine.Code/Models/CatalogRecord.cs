namespace CoffeeMachine.Code;

public record CatalogRecord(Products DrinkType, string DrinkCode, double Price)
{
    public readonly Products DrinkType = DrinkType;
    public readonly string DrinkCode = DrinkCode;
    public readonly double Price = Price;
}