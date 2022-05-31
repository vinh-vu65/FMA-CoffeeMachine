namespace CoffeeMachine.Code.Models;

public record FulfilledDrinkOrder(DrinkOrder DrinkOrder, DateTime TimePurchased, decimal Price);