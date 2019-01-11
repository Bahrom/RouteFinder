using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace murano_homework.Models
{
    public class Airport
    {
        /// <summary>
        /// Name of airport. May or may not contain the City name.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Alias of the airline. For example, All Nippon Airways is commonly known as "ANA".
        /// </summary>
        public string alias { get; set; }

        /// <summary>
        /// Main city served by airport. May be spelled differently from Name.
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// Country or territory where airport is located. See countries.dat to cross-reference to ISO 3166-1 codes.
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// Decimal degrees, usually to six significant digits. Negative is South, positive is North.
        /// </summary>
        public float latitude { get; set; }

        /// <summary>
        /// Decimal degrees, usually to six significant digits. Negative is West, positive is East.
        /// </summary>
        public float longitude { get; set; }

        /// <summary>
        /// In feet.
        /// </summary>
        public int altitude { get; set; }


    }
}