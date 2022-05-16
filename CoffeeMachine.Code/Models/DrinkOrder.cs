using CoffeeMachine.Code.Interfaces;

namespace CoffeeMachine.Code;

public class DrinkOrder : IDrinkOrder
{
    public Products DrinkType { get; set; }
    public int Sugars { get; set; }

    public DrinkOrder(Products drinkType, int sugars)
    {
        DrinkType = drinkType;
        Sugars = sugars;
    }
}