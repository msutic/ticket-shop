using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketShopMVC.Models
{
    public class Ticket
    {
        public string Username { get; set; }
        public string Id { get; set; }
        public Manifestation ReservedFor { get; set; }
        public DateTime DateAndTime { get; set; }
        public int Price { get; set; }
        public User Customer { get; set; }
        public TicketStatus Status { get; set; }
        public TicketType Type { get; set; }

        public Ticket()
        {

        }

        public Ticket(string id, Manifestation reservedFor, DateTime dateAndTime, int price, string username, TicketStatus status, TicketType type)
        {
            Customer = new User();
            Id = id;
            ReservedFor = reservedFor;
            DateAndTime = dateAndTime;
            Price = price;
            Username = username;
            Status = status;
            Type = type;
        }
    }
}