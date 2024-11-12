using FlightInventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlightInventoryManagement.Services
{
    public interface IOrderService
    {
        List<Order> GetOrders();
    }
    public class OrderService : IOrderService
    {
        public List<Order> GetOrders()
        {
            var orders = GetOrdersFromFile();
            if (orders == null) return new List<Order>();
            return orders.Select(x => new Order
            {
                Destination = x.Value.Destination,
                Name = x.Key
            }).ToList();
        }
        private Dictionary<string, OrderDTO>? GetOrdersFromFile()
        {
            try
            {
                var filePath = "coding-assigment-orders.json";

                var jsonString = File.ReadAllText(filePath);

                var ordersDictionary = JsonSerializer.Deserialize<Dictionary<string, OrderDTO>>(jsonString);
                return ordersDictionary;
            }
            catch 
            { 
                return null;
            }
           
        }
    }
}
