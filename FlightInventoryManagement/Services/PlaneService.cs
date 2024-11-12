using FlightInventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightInventoryManagement.Services
{

    public interface IAirPlaneService
    {
        List<AirPlane> GetAirPlanes();
    }
    public class AirPlaneService : IAirPlaneService
    {
        private readonly int NoOfPlanes = 3;
        private readonly int AirplaneCapacity = 20;
        private readonly IAirportService _airportService;
        public AirPlaneService(IAirportService airportService) 
        {
            _airportService = airportService;
        }
        public List<AirPlane> GetAirPlanes()
        {
            var list = new List<AirPlane>();
            var airports = _airportService.GetAirports();
            for(int i = 0;i < NoOfPlanes; i++)
            {
                Airport? airport = null;
                if (airports.Count >= i) 
                {
                    //we assume montreal airport is 0, so we begin from the next in the list
                    airport = airports[i+1];
                };
                list.Add(new AirPlane(i + 1, AirplaneCapacity, airport));
            }
            return list;
        }
    }
}
