using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketShopMVC.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public Manifestation ReservedFor { get; set; }
        public DateTime DateAndTime { get; set; }
        public int Price { get; set; }
        public User Customer { get; set; }
        public TicketStatus Status { get; set; }
        public TicketType Type { get; set; }

        public Ticket()
        {

        }

        public Ticket(int id, Manifestation reservedFor, DateTime dateAndTime, int price, User customer, TicketStatus status, TicketType type)
        {
            Id = id;
            ReservedFor = reservedFor;
            DateAndTime = dateAndTime;
            Price = price;
            Customer = customer;
            Status = status;
            Type = type;
        }
    }
}