using FlightInventoryManagement.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FlightInventoryManagement
{
    internal class Program
    {

        static void Main(string[] args)
        {
            var endApp = false;
            var serviceCollection = SetUP();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var schedulingService = serviceProvider.GetRequiredService<IFlightScheduleService>();
            schedulingService.LoadDailySchedule();

            do
            {
                Console.WriteLine("\nEnter one of the following commands\n" +
                    $"{Commands.ListFlights} (To List Scheduled Flights)\n" +
                    $"{Commands.GenerateOrderItenary} (To List Generated Order Itenary)\n" +
                    $"{Commands.EndCommand} to (To End Program)\n");

                var input = Console.ReadLine();
                var trimmed = string.IsNullOrWhiteSpace(input) ? "" : input.Trim();
                switch (trimmed)
                {
                    case Commands.ListFlights:
                        schedulingService.PrintSchedule();
                        break;
                    case Commands.GenerateOrderItenary:
                        schedulingService.GenerateItenary();
                        break;
                    case Commands.EndCommand:
                        endApp = true;
                        break;
                    default:
                        Console.WriteLine("Invalid Command");
                        break;
                }



            } while (!endApp);



        }
        static ServiceCollection SetUP()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IAirportService, AirportService>();
            serviceCollection.AddSingleton<IFlightScheduleService, FlightScheduleService>();
            return serviceCollection;
        }
    }
}
