using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace murano_homework.Models
{
    public class Airline
    {
        /// <summary>
        /// Name of the airline.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Alias of the airline. For example, All Nippon Airways is commonly known as "ANA".
        /// </summary>
        public string alias { get; set; }

        /// <summary>
        /// True if the airline is or has until recently been operational, otherwise it is defunct.
        /// </summary>
        public bool active { get; set; }
    }
}