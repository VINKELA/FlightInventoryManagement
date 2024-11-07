using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightInventoryManagement.Models
{
    public class AirPlane
    {
        public  int FlightNumber {get; set;}
        public  int Capacity { get; set; }
        public AirPlane(int flightNumber, int capacity)
        {
            FlightNumber = flightNumber;
            Capacity = capacity;
        }
    } 

    
}
