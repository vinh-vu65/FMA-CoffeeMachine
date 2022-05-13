namespace CoffeeMachine.Code;

public record CatalogRecord(Products DrinkType, string DrinkCode)
{
    public readonly Products DrinkType = DrinkType;
    public readonly string DrinkCode = DrinkCode;
}