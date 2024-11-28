using FlightInventoryManagement.Models;
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
            //This loads the Flight Schedule into a graph data structure for easy transerval 

            schedulingService.LoadDailySchedule();

            do
            {
                Console.WriteLine("\nEnter one of the following commands\n\n" +
                    $"{Commands.ListFlights} (To List Scheduled Flights)\n" +
                    $"{Commands.GenerateOrderItenary} (To Generate and List Generated Order Itenary)\n" +
                    $"{Commands.EndCommand}  (To End Program)\n\n");

                var input = Console.ReadLine();
                var trimmed = string.IsNullOrWhiteSpace(input) ? "" : input.Trim().ToLower();
                switch (trimmed)
                {
                    case Commands.ListFlights:
                        schedulingService.PrintSchedule();
                        break;
                    case Commands.GenerateOrderItenary:
                        schedulingService.GenerateItenary();
                        break;
                    case Commands.ListFlightOrders:
                        schedulingService.GetFlightOrders(1);
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
            serviceCollection.AddSingleton<IOrderService, OrderService>();
            serviceCollection.AddTransient<IAirPlaneService, AirPlaneService>();
            serviceCollection.AddTransient<IDayService, DayService>();
            return serviceCollection;
        }
    }
}
