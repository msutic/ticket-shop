using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketShopMVC.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public Nullable<Gender> Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Role Role { get; set; }
        public List<Ticket> ResrvedTickets { get; set; }
        public List<Manifestation> Manifestations { get; set; }
        public int EarnedPoints { get; set; }
        public UserType UserType { get; set; }
        public bool IsDeleted { get; set; }

        public User()
        {
            UserType = new UserType("Regular", 0, 1000);
            Gender = null;
            DateOfBirth = new DateTime(01, 01, 0001);
            Role = Role.CUSTOMER;
            EarnedPoints = 0;
            IsDeleted = false;
        }

        //FOR ADMINS
        public User(string username, string password, string name, string lastname, Gender gender, DateTime dateOfBirth, Role role)
        {
            Username = username;
            Password = password;
            Name = name;
            Lastname = lastname;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            Role = role;
            IsDeleted = false;
        }

        //FOR CUSTOMERS
        public User(string username, string password, string name, string lastname, Gender gender, DateTime dateOfBirth, Role role, 
                    List<Ticket> resrvedTickets, int earnedPoints, UserType userType)
        {
            Username = username;
            Password = password;
            Name = name;
            Lastname = lastname;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            Role = role;
            ResrvedTickets = resrvedTickets;
            EarnedPoints = earnedPoints;
            UserType = userType;
        }

        //FOR SALESMEN
        public User(string username, string password, string name, string lastname, Gender gender, DateTime dateOfBirth, Role role,
                    List<Manifestation> manifestations)
        {
            Username = username;
            Password = password;
            Name = name;
            Lastname = lastname;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            Role = role;
            Manifestations = manifestations;
        }

        public override bool Equals(object obj)
        {
            return ((User)obj).Username.Equals(this.Username);
        }
    }
}