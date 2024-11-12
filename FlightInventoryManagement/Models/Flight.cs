using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightInventoryManagement.Models
{
    public class Flight
    {
        public required Airport Departure { get; set; }
        public required  Airport Arrival { get; set; }
        public required Day Day {  get; set; }
        public required Airplane Plane { get; set; }
        public required TimeOnly Time { get; set; }
        
    }
}
