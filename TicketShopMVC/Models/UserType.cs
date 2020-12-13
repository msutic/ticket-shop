using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketShopMVC.Models
{
    public class UserType
    {
        public string TypeName { get; set; }
        public int Discount { get; set; }
        public int PointsNeeded { get; set; }

        public UserType()
        {

        }

        public UserType(string typeName, int discount, int pointsNeeded)
        {
            TypeName = typeName;
            Discount = discount;
            PointsNeeded = pointsNeeded;
        }
    }
}