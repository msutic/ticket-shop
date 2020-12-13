using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace TicketShopMVC.Models
{
    public class FileOperations
    {
        public static List<User> ReadUsers(string path)
        {
            List<User> users = new List<User>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');

                bool deleted;

                DateTime birthDate = new DateTime(
                    Int32.Parse(tokens[5].Split('/')[2]),
                    Int32.Parse(tokens[5].Split('/')[1]),
                    Int32.Parse(tokens[5].Split('/')[0]));

                User u = new User();

                if(tokens[6] == "CUSTOMER")
                {
                    if (tokens[10] == "DELETED")
                    {
                        deleted = true;
                    } else
                    {
                        deleted = false;
                    }
                    List<Ticket> reservedTickets = new List<Ticket>();

                    //string[] ids = tokens[7].Split(',');

                    //for(int i = 0; i < ids.Length; i++)
                    //{

                    //}

                    UserType type = new UserType(tokens[9].Split(',')[0], int.Parse(tokens[9].Split(',')[1]), int.Parse(tokens[9].Split(',')[2]));
                    u = new User(tokens[0], tokens[1], tokens[2], tokens[3], (Gender)Enum.Parse(typeof(Gender), tokens[4]), birthDate, (Role)Enum.Parse(typeof(Role), tokens[6]), reservedTickets, int.Parse(tokens[8]), type);
                    u.IsDeleted = deleted;
                } else if(tokens[6] == "SALESMAN")
                {
                    string manifes = tokens[7];
                    string[] str = manifes.Split(',');

                    List<Manifestation> manifestations = ReadManifestations("~/App_Data/manifestations.txt");
                    List<Manifestation> manifestationList = new List<Manifestation>();

                    foreach(Manifestation m in manifestations)
                    {
                        foreach(string s in str)
                        {
                            if (s.Equals(m.Name))
                                manifestationList.Add(m);
                        }
                    }
                    //treba da dodam i za prodavca DELETED
                    if(tokens[8] == "DELETED")
                    {
                        deleted = true;
                    } else
                    {
                        deleted = false;
                    }
                    
                    u = new User(tokens[0], tokens[1], tokens[2], tokens[3], (Gender)Enum.Parse(typeof(Gender), tokens[4]), birthDate, (Role)Enum.Parse(typeof(Role), tokens[6]), manifestationList);
                    u.IsDeleted = deleted;

                } else if(tokens[6] == "ADMINISTRATOR")
                {
                    u = new User(tokens[0], tokens[1], tokens[2], tokens[3], (Gender)Enum.Parse(typeof(Gender), tokens[4]), birthDate, (Role)Enum.Parse(typeof(Role), tokens[6]));
                }
                

                users.Add(u);

            }

            sr.Close();
            stream.Close();

            return users;
        }

        public static void SaveUser(User user)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/users.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            List<Manifestation> manifestations = ReadManifestations("~/App_Data/manifestations.txt");
            string manifestationList = "";

            foreach(Manifestation m in manifestations)
            {
                foreach(Manifestation userM in user.Manifestations)
                {
                    if (m.Name.Equals(userM.Name))
                    {
                        manifestationList += $"{m.Name},";
                    }
                }
            }

            //proveri da li je kupac ili prodavac, jer ce drugaciji biti zapis...
            if (user.Role.Equals(Role.CUSTOMER))
            {
                sw.WriteLine($"{user.Username};{user.Password};{user.Name};{user.Lastname};{user.Gender};{user.DateOfBirth.ToString("dd/M/yyyy")};{user.Role};LIST;{user.EarnedPoints};{user.UserType.TypeName},{user.UserType.Discount},{user.UserType.PointsNeeded};");

            } else if (user.Role.Equals(Role.SALESMAN))
            {
                sw.WriteLine($"{user.Username};{user.Password};{user.Name};{user.Lastname};{user.Gender};{user.DateOfBirth.ToString("dd/M/yyyy")};{user.Role};{manifestationList};");

            }

            sw.Close();
            stream.Close();
        }


        public static List<Manifestation> ReadManifestations(string path)
        {
            List<Manifestation> manifestations = new List<Manifestation>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            CultureInfo ci = new CultureInfo("de-DE");
            string line = "";
            while((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                DateTime date = DateTime.Parse(tokens[3], ci);
                Place place = new Place(tokens[6].Split(',')[0], tokens[6].Split(',')[1], int.Parse(tokens[6].Split(',')[2]));
                Manifestation m = new Manifestation(tokens[0], tokens[1], int.Parse(tokens[2]), date, int.Parse(tokens[4]), (Status)Enum.Parse(typeof(Status), tokens[5]), place);
                manifestations.Add(m);
            }

            sr.Close();
            stream.Close();

            return manifestations;
        }

        public static void SaveManifestation(Manifestation manifestation)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/manifestations.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);


            sw.WriteLine($"{manifestation.Name};{manifestation.ManifestationType};{manifestation.numOfSeats};{manifestation.DateAndTime.ToString("dd/MM/yyyy hh:mm tt")};{manifestation.PriceRegular};{manifestation.Status};{manifestation.Place.Address},{manifestation.Place.City},{manifestation.Place.ZIPCode}");

            sw.Close();
            stream.Close();
        }

        public static void OverwriteUsers(List<User> users)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/users.txt");
            List<Manifestation> manifestations = ReadManifestations("~/App_Data/manifestations.txt");

            string deleted = "";
            string content = "";
            string manifestList = "";
            foreach(User user in users)
            {
                if (user.IsDeleted)
                {
                    deleted = "DELETED";
                }
                else{
                    deleted = "";
                }
                if (user.Role.Equals(Role.ADMINISTRATOR))
                {
                    content += user.Username + ";" + user.Password + ";" + user.Name + ";" + user.Lastname + ";" + user.Gender + ";" + user.DateOfBirth.ToString("dd/MM/yyyy") + ";" + user.Role + "\n";

                } else if (user.Role.Equals(Role.CUSTOMER))
                {
                    content += user.Username + ";" + user.Password + ";" + user.Name + ";" + user.Lastname + ";" + user.Gender + ";" + user.DateOfBirth.ToString("dd/MM/yyyy") + ";" + user.Role + ";" + "LIST" + ";" + user.EarnedPoints + ";" + user.UserType.TypeName + "," + user.UserType.Discount + "," + user.UserType.PointsNeeded + ";" + deleted + "\n";

                } else if (user.Role.Equals(Role.SALESMAN))
                {
                    foreach(Manifestation m in manifestations)
                    {
                        foreach(Manifestation userM in user.Manifestations)
                        {
                            if (m.Name.Equals(userM.Name))
                            {
                                manifestList += $"{m.Name},";
                            }
                        }
                    }
                    content += $"{user.Username};{user.Password};{user.Name};{user.Lastname};{user.Gender};{user.DateOfBirth.ToString("dd/M/yyyy")};{user.Role};{manifestList};\n";
                }
            }

            File.WriteAllText(path, content);
        }

        public static void OverwriteManifestations(List<Manifestation> manifestations)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/manifestations.txt");

            string content = "";

            foreach(Manifestation m in manifestations)
            {
                content += $"{m.Name};{m.ManifestationType};{m.numOfSeats};{m.DateAndTime.ToString("dd/MM/yyyy hh:mm tt")};{m.PriceRegular};{m.Status};{m.Place.Address},{m.Place.City},{m.Place.ZIPCode}\n";
            }

            File.WriteAllText(path, content);
        }
    }
}