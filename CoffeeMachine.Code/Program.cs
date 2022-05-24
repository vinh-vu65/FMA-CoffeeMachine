using CoffeeMachine.Code.Controller;
using CoffeeMachine.Code.Models;
using CoffeeMachine.Code.Services;
using NSubstitute;

IDrinkMaker drinkMaker = Substitute.For<IDrinkMaker>();
IReportGenerator reportGenerator = new ReportGenerator();
var hotChoc = new DrinkOrder(DrinkType.HotChocolate, 2, false);
var coffee = new DrinkOrder(DrinkType.Coffee, 2, true);
var tea = new DrinkOrder(DrinkType.Tea, 2, false);
var OJ = new DrinkOrder(DrinkType.OrangeJuice,0 , false);

var catalog = new DrinksCatalog();
var builder = new ProtocolBuilder();
var sut = new DrinkMachineController(catalog, builder, drinkMaker, reportGenerator);

sut.ManageDrinkOrder(hotChoc, 10m);
sut.ManageDrinkOrder(coffee, 10m);
sut.ManageDrinkOrder(tea, 10m);
sut.ManageDrinkOrder(OJ, 10m);

sut.PrintDrinkHistory();