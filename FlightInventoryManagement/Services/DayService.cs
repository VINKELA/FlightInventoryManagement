using FlightInventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightInventoryManagement.Services
{

    public interface IDayService
    {
        List<Day> GetDays();
    }
    public class DayService : IDayService
    {
        private readonly int NoOFDays = 2;
        public List<Day> GetDays()
        {
            var list = new List<Day>();
            for (int i = 0; i < NoOFDays; i++)
            {
                list.Add(new Day { Id = i+1 });
            }
            return list;
        }
    }
}
