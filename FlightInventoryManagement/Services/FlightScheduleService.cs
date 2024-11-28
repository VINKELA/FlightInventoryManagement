using FlightInventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static FlightInventoryManagement.Models.OrderDTO;

namespace FlightInventoryManagement.Services
{
    interface IFlightScheduleService
    {
        public void PrintSchedule();
        public void LoadDailySchedule();
        public void GenerateItenary();
        public void GetFlightOrders(int flightnumber);

    }
    public class FlightScheduleService : IFlightScheduleService
    {
        private readonly IAirportService _airportService;
        private readonly IAirPlaneService _airplaneService;
        private readonly IDayService _dayService;
        private readonly IOrderService _orderService;
        public readonly Dictionary<int, Dictionary<string, List<Flight>>> DailyFlights = new Dictionary<int, Dictionary<string, List<Flight>>>();
        public FlightScheduleService(IAirportService airportService, IAirPlaneService airPlane, IDayService dayService, IOrderService orderService)
        {
            _airportService = airportService;
            _dayService = dayService;
            _airplaneService = airPlane;
            _orderService = orderService;   
        }
        //Load Data into a graph data structure
        public void LoadDailySchedule()
        {
            var airports = _airportService.GetAirports();
            var days = _dayService.GetDays();
            for (int i = 0; i < days.Count; i++)
            {
                var day = days[i];
                DailyFlights.Add(day.Id, ScheduleFlights(airports, day));
            }
        }
        private Dictionary<string, List<Flight>> ScheduleFlights(List<Airport> airports, Day day)
        {
            var airplanes = _airplaneService.GetAirPlanes();
            var flights = new Dictionary<string, List<Flight>>();
            var countOfAirports = airports.Count;
            var montrealAirport = airports[0];
            var planeCount = airplanes.Count;
            for (int j = 0; j < planeCount; j++)
            {
                var plane = airplanes[j];
                //ignore if we have reached the number of available airports
                var airport = plane.Airport;
                if(airport != null)
                {
                    var outBoundFlight = new Flight
                    {
                        Day = day,
                        Departure = montrealAirport,
                        Arrival = airport,
                        Plane = plane,
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
            //schedule all planes
            return flights;
        }
        //print schedule for all days
        public void PrintSchedule()
        {
            var days = _dayService.GetDays();
            for (int i = 0; i < days.Count; i++)
            {
                var dailyFlight = DailyFlights[days[i].Id];
                foreach (var airPortflights in dailyFlight)
                {
                    foreach (var flight in airPortflights.Value)
                    {
                        Console.WriteLine($"Flight: {flight.Plane.FlightNumber}, " +
                            $"departure: {flight.Departure.Code}, " +
                            $"arrival: {flight.Arrival.Code}, day: {flight.Day.Id}");
                    }
                }
            }
        }
        //generate itenary based on the orders
        public void GenerateItenary()
        {
            var orders = _orderService.GetOrders();
            var days = _dayService.GetDays();
            if (orders == null) return;
            //for each order, try to fufil across the 2 days
            foreach (var order in orders)
            {
                var orderfufilled = false;
                if (!string.IsNullOrEmpty(order.Name))
                {
                    //iterate through the days
                    for (int i = 0; i < days.Count && !orderfufilled; i++)
                    {
                        var day = days[i];
                        var airports = DailyFlights[day.Id];
                        var destination = order.Destination;
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
                                Console.WriteLine($"order:{order.Name}, " +
                                $"flightNumber: {currentFlight.Plane.FlightNumber}," +
                                $" departure: {currentFlight.Departure.Code}, " +
                                $"arrival: {currentFlight.Arrival.Code}, " +
                                $"priority: {order.Priority}, " +
                                $"day: {currentFlight.Day.Id}");
                                currentFlight.Orders.Add(order);
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
                    Console.WriteLine($"order: {order.Name}, flightNumber: " +
                        $"not scheduled");
                }
            }
        }
        public void GetFlightOrders(int flightnumber)
        {
            var days = _dayService.GetDays();
            var list = new List<Flight>();
            for(int i = 0; i < days.Count; i++)
            {
                var graph = DailyFlights[i + 1];
                foreach(var key in graph.Keys)
                {
                    var flights = graph[key];
                    foreach (var flight in flights) 
                    {
                        var orders = flight.Orders;
                        foreach (var order in orders)
                        {
                            if (flight.Plane.FlightNumber == flightnumber)
                            {
                                Console.WriteLine($"order:{order.Name}, " +
                                   $"flightNumber: {flight.Plane.FlightNumber}," +
                                   $" departure: {flight.Departure.Code}, " +
                                   $"arrival: {flight.Arrival.Code}, " +
                                   $"day: {flight.Day.Id}");
                            }
                        }
                       
                    }
                }
            }
        }
    }
}
