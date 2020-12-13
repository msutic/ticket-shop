using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketShopMVC.Models
{
    public class Location
    {
        public double GeoLength { get; set; }
        public double GeoWidth { get; set; }
        public Place Place { get; set; }

        public Location()
        {

        }

        public Location(double geoLength, double geoWidth, Place place)
        {
            GeoLength = geoLength;
            GeoWidth = geoWidth;
            Place = place;
        }
    }
}