using FlightInventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightInventoryManagement.Services
{
    public interface IAirportService
    {
        List<Airport> GetAirports();

    }
    public class AirportService: IAirportService
    {

        public  List<Airport> GetAirports()
        {
            var airports = new List<Airport>()
            {
               new() {
                   Name = "Montreal",
                   Code = "YUL"
               },
               new() {
                   Name = "Toronto",
                   Code = "YYZ"
               },
               new() {
                   Name = "Calgary",
                   Code = "YYC"
               },
                new() {
                   Name = "Vancouver",
                   Code = "YVR"
               }
            };

            return airports;
        }

    }
}
