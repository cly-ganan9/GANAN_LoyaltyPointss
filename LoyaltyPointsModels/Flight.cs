using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPointsModels
{
    public class Flight
    {
        public int FlightId { get; set; }
        public string Destination { get; set; }
        public decimal RoundTripPrice { get; set; }
    }
}