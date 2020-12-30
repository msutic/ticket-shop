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
        public List<string> TicketIds { get; set; }
        public List<string> ManifestationNames { get; set; }
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
                    List<string> ticketIds, int earnedPoints, UserType userType)
        {
            TicketIds = ticketIds;
            Username = username;
            Password = password;
            Name = name;
            Lastname = lastname;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            Role = role;
            ResrvedTickets = new List<Ticket>();
            EarnedPoints = earnedPoints;
            UserType = userType;
        }

        //FOR SALESMEN
        public User(string username, string password, string name, string lastname, Gender gender, DateTime dateOfBirth, Role role,
                    List<string> manifestationNames)
        {
            ManifestationNames = manifestationNames;
            Username = username;
            Password = password;
            Name = name;
            Lastname = lastname;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            Role = role;
            Manifestations = new List<Manifestation>();
        }

        public override bool Equals(object obj)
        {
            return ((User)obj).Username.Equals(this.Username);
        }
    }
}