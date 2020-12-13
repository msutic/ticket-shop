using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketShopMVC.Models
{
    public class Manifestation
    {
        public string Name { get; set; }
        public string ManifestationType { get; set; }
        public int numOfSeats { get; set; }
        public DateTime DateAndTime { get; set; }
        public int PriceRegular { get; set; }
        public Status Status { get; set; }
        public Place Place { get; set; }
        public Poster Poster { get; set; }

        public Manifestation()
        {

        }

        public Manifestation(string name, string manifestationType, int numOfSeats, DateTime dateAndTime, int priceRegular, Status status, Place place)
        {
            Name = name;
            ManifestationType = manifestationType;
            this.numOfSeats = numOfSeats;
            DateAndTime = dateAndTime;
            PriceRegular = priceRegular;
            Status = status;
            Place = place;
            //Poster = poster;
        }
    }
}