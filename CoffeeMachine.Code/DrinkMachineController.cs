namespace CoffeeMachine.Code;

public class DrinkMachineController
{
    public string DrinkMakerProtocol { get; set; }
    
    public void CreateDrinkMakerProtocol(IDrink drinkRequested)
    {
        DrinkMakerProtocol = drinkRequested.DrinkCode;
    }
}