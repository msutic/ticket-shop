using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketShopMVC.Models
{
    public class Comment
    {
        public User Customer { get; set; }
        public Manifestation Manifestation { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }

        public Comment()
        {

        }

        public Comment(User customer, Manifestation manifestation, string text, int rating)
        {
            Customer = customer;
            Manifestation = manifestation;
            Text = text;
            Rating = rating;
        }
    }
}