using FlightInventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlightInventoryManagement.Services
{
    interface IFlightScheduleService
    {
        public void PrintSchedule();
        public void LoadDailySchedule();
        public void GenerateItenary();


    }
    public class FlightScheduleService : IFlightScheduleService
    {
        private readonly int NoOFDays = 2;
        private readonly int NoOfPlanes = 3;
        private readonly int AirplaneCapacity = 20;
        private readonly IAirportService _airportService;
        public readonly Dictionary<int, Dictionary<string, List<Flight>>> DailyFlights = new Dictionary<int, Dictionary<string, List<Flight>>>();

        public FlightScheduleService(IAirportService airportService)
        {
            _airportService = airportService;
        }
        //Load Data into a graph data structure
        public void LoadDailySchedule()
        {
            var airports = _airportService.GetAirports();
            for (int i = 1; i <= NoOFDays; i++)
            {
                DailyFlights.Add(i, ScheduleFlights(airports, NoOfPlanes, i));
            }

        }
        private Dictionary<string, List<Flight>> ScheduleFlights(List<Airport> airports, int countOFPlanes, int day)
        {

            var flights = new Dictionary<string, List<Flight>>();
            var countOfAirports = airports.Count;
            //Each flight does a round trip of montreal Airport
            var montrealAirport = airports[0];
            //schedule all planes
            for (int i = 0; i < countOFPlanes; i++)
            {
                var fligtNumber = i + 1;
                //same plane will go to and fro
                var plane1 = new AirPlane(fligtNumber, AirplaneCapacity);
                //ignore if we have reached the number of available airports
                if (countOfAirports > i + 1)
                {
                    var airport = airports[i + 1];
                    var outBoundFlight = new Flight
                    {
                        Day = day,
                        Departure = montrealAirport,
                        Arrival = airport,
                        Plane = plane1,
                        Time = TimeOnly.Parse("12:00")
                    };
                    if (!flights.TryGetValue(outBoundFlight.Arrival.Code, out List<Flight>? value))
                    {
                        value = new List<Flight>();
                        flights[outBoundFlight.Arrival.Code] = value;
                    }

                    value.Add(outBoundFlight);
                  
                }
            }
            return flights;
        }
        //print schedule for all days
        public void PrintSchedule()
        {
            for (int i = 1; i <= NoOFDays; i++)
            {
                var dailyFlight = DailyFlights[i];
                foreach (var airPortflights in dailyFlight)
                {
                    foreach (var flight in airPortflights.Value)
                    {
                        Console.WriteLine($"Flight: {flight.Plane.FlightNumber}, departure: {flight.Departure.Code}, arrival: {flight.Arrival.Code}, day: {flight.Day}");
                    }

                }

            }
        }
        //generate itenary based on the orders
        public void GenerateItenary()
        {
            var orders = GetOrders();
            if (orders == null) return;
            //for each order, try to fufil across the 2 days
            foreach (var order in orders)
            {
                var orderfufilled = false;
                if (!string.IsNullOrEmpty(order.Key))
                {
                    //iterate through the days
                    for (int i = 1; i <= NoOFDays && !orderfufilled; i++)
                    {
                        var airports = DailyFlights[i];
                        var destination = order.Value.Destination;
                        if (!airports.ContainsKey(destination))
                        {
                            break;
                        }
                        var flightsToDestinationToday = airports[destination];
                        var countOfFlight = flightsToDestinationToday.Count;
                        var j = 0;
                        while (!orderfufilled && j < countOfFlight)
                        {
                            var currentFlight = flightsToDestinationToday[j];
                            //fufill Order
                            if (currentFlight.Plane.Capacity > 0)
                            {
                                Console.WriteLine($"order:{order.Key.Trim()}, flightNumber: {currentFlight.Plane.FlightNumber}, departure: {currentFlight.Departure.Code}, arrival: {currentFlight.Arrival.Code}, day: {currentFlight.Day}");
                                currentFlight.Plane.Capacity--;
                                orderfufilled = true;
                                break;
                            }
                            j++;
                        }

                    }
                }
                //order is not fufilled
                if (!orderfufilled)
                {
                    Console.WriteLine($"order: {order.Key}, flightNumber: not scheduled");
                }
            }

        }
        public Dictionary<string, Order>? GetOrders()
        {
            var filePath = "coding-assigment-orders.json";

            var jsonString = File.ReadAllText(filePath);

            var ordersDictionary = JsonSerializer.Deserialize<Dictionary<string, Order>>(jsonString);
            return ordersDictionary;
        }

    }
}
