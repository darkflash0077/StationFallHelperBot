// See https://aka.ms/new-console-template for more information
using StationFallHelperBot;

Console.WriteLine("Hello, World!");
var bot = new MainBOt();
while (bot.IsActive) {
    Thread.Sleep(5000);
}
