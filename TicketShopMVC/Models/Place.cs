using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketShopMVC.Models
{
    public class Place
    {
        public string Address { get; set; }
        public string City { get; set; }
        public int ZIPCode { get; set; }

        public Place()
        {

        }

        public Place(string address, string city, int zIPCode)
        {
            Address = address;
            City = city;
            ZIPCode = zIPCode;
        }
    }
}