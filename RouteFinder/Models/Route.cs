using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RouteFinder.Models
{
   
    public class Route
    {
        /// <summary>
        /// 2-letter (IATA) or 3-letter (ICAO) code of the airline
        /// </summary>
        public string airline { get; set; }

        /// <summary>
        /// 3-letter (IATA) or 4-letter (ICAO) code of the source airport
        /// </summary>
        public string srcAirport { get; set; }

        /// <summary>
        /// 3-letter (IATA) or 4-letter (ICAO) code of the destination airport
        /// </summary>
        public string destAirport { get; set; }
    }
}