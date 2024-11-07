using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightInventoryManagement.Models
{
    public class Order
    {
        [JsonPropertyName("destination")]
        public string? Destination { get; set; }
        public bool Fufilled { get; set; } = false;
        public class Orders
        {
            public Dictionary<string, Order>? OrdersDictionary { get; set; }
        }
    }
}
