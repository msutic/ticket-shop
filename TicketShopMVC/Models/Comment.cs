using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketShopMVC.Models
{
    public class Comment
    {
        public string User { get; set; }
        public string ManifestationName { get; set; }
        public User Customer { get; set; }
        public Manifestation Manifestation { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public bool Verified { get; set; }

        public Comment()
        {

        }

        public Comment(string user, string manifestation, string text, int rating)
        {
            User = user;
            ManifestationName = manifestation;
            Customer = new User();
            Manifestation = new Manifestation();
            Text = text;
            Rating = rating;
        }
    }
}