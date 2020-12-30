using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketShopMVC.Models;

namespace TicketShopMVC.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Info()
        {
            return View();
        }

        public ActionResult Edit(string username)
        {
            List<User> users = (List<User>)HttpContext.Application["users"];
            User user = new User();
            Index currentId = new Index();
            for(int i = 0; i < users.Count; i++)
            {
                if (users[i].Username.Equals(username))
                {
                    user = users[i];
                    currentId.SavedIndex = i;
                    HttpContext.Application["currentId"] = currentId;
                    HttpContext.Application["userCurrent"] = user;
                    return View(user);
                }
            }

            return View();
        }

        public ActionResult MiddleAction()
        {
            User u = (User)HttpContext.Application["userCurrent"];
            ViewBag.Message = (string)TempData["Message"];

            return View(u);
        }

        [HttpPost]
        public ActionResult Modify(User user)
        {
            List<User> users = (List<User>)HttpContext.Application["users"];
            Index currentId = (Index)HttpContext.Application["currentId"];

            User u = (User)HttpContext.Application["userCurrent"];
            user.Role = u.Role;
            user.Username = u.Username;
            

            if(user.Password == null)
            {
                TempData["Message"] = "Password is required";
                return RedirectToAction("MiddleAction", "Profile");
            }
            if (user.Name == null)
            {
                TempData["Message"] = "Name is required";
                return RedirectToAction("MiddleAction", "Profile");
            }
            if (user.Lastname == null)
            {
                TempData["Message"] = "Lastname is required";
                return RedirectToAction("MiddleAction", "Profile");
            }
            if (user.Gender == null)
            {
                TempData["Message"] = "Gender is required";
                return RedirectToAction("MiddleAction", "Profile");
            }
            if (user.DateOfBirth.Equals(new DateTime(01, 01, 0001)))
            {
                TempData["Message"] = "DateOfBirth is required";
                return RedirectToAction("MiddleAction", "Profile");
            }

            
            if (user.Role.Equals(Role.CUSTOMER))
            {
                user.EarnedPoints = u.EarnedPoints;
                user.UserType = u.UserType;
                user.ResrvedTickets = u.ResrvedTickets;
            } else if (user.Role.Equals(Role.SALESMAN))
            {
                user.Manifestations = u.Manifestations;
            }
            
            for(int i = 0; i < users.Count; ++i)
            {
                if (users[i].Username == user.Username)
                {
                    users[i] = user;
                    break;
                }
            }
            FileOperations.OverwriteUsers(users);

            Session["user"] = user;

            return RedirectToAction("Info", "Profile");

        }

        public ActionResult ShowTickets()
        {
            User user = (User)Session["User"];
            List<Ticket> tickets = (List<Ticket>)HttpContext.Application["tickets"];
            List<Ticket> result = new List<Ticket>();

            if (user.Role.Equals(Role.CUSTOMER))
            {
                //show all of HIS tickets - RESERVED AND CANCELLED 
                foreach(Ticket t in tickets)
                {
                    if (t.Customer.Username.ToLower().Equals(user.Username.ToLower()))
                    {
                        result.Add(t);
                    }
                }
                HttpContext.Application["myTickets"] = result;
                HttpContext.Application["originalT"] = result;

            } else if (user.Role.Equals(Role.ADMINISTRATOR))
            {
                result = tickets;
            }
            else if (user.Role.Equals(Role.SALESMAN))
            {
                if(user.Manifestations != null)
                {
                    foreach (Ticket t in tickets)
                    {
                        foreach (Manifestation m in user.Manifestations)
                        {
                            if (t.ReservedFor.Name.ToLower().Equals(m.Name.ToLower()))
                            {
                                if (t.Status.Equals(TicketStatus.RESERVED))
                                {
                                    result.Add(t);
                                }
                            }
                        }
                    }
                }
                
            }
            

            return View(result);
        }

        public ActionResult ShowCustomers()
        {
            User user = (User)Session["User"];
            List<Ticket> tickets = (List<Ticket>)HttpContext.Application["tickets"];
            List<User> users = (List<User>)HttpContext.Application["users"];
            List<User> result = new List<User>();
            List<string> ret = new List<string>();

            if(user.Manifestations != null)
            {
                foreach (Ticket t in tickets)
                {
                    foreach (Manifestation m in user.Manifestations)
                    {
                        if (t.ReservedFor.Name.Equals(m.Name))
                        {
                            ret.Add(t.Customer.Username);
                        }
                    }
                }
                foreach (User u in users)
                {
                    foreach (string s in ret)
                    {
                        if (u.Username.Equals(s))
                        {
                            result.Add(u);
                        }
                    }
                }
            }
            
            return View(result);
        }

        public ActionResult Search(string name, string priceFrom, string priceTo, DateTime dateFrom, DateTime dateTo)
        {
            List<Ticket> tickets = (List<Ticket>)HttpContext.Application["myTickets"];
            List<Ticket> searchedFor = new List<Ticket>();

            if(name == "" && priceFrom == "" && priceTo == "")
            {
                foreach(Ticket t in tickets)
                {
                    if (t.DateAndTime >= dateFrom && t.DateAndTime <= dateTo)
                        searchedFor.Add(t);
                }
                HttpContext.Application["searched"] = searchedFor;
                return View("SearchResult", searchedFor);
            }

            if (name != "" && priceFrom == "" && priceTo == "")
            {
                foreach (Ticket t in tickets)
                {
                    if (t.DateAndTime >= dateFrom && t.DateAndTime <= dateTo && t.ReservedFor.Name.ToLower().Equals(name.ToLower()))
                        searchedFor.Add(t);
                }
                HttpContext.Application["searched"] = searchedFor;
                return View("SearchResult", searchedFor);
            }

            if (name == "" && priceFrom != "" && priceTo == "")
            {
                foreach (Ticket t in tickets)
                {
                    if (t.DateAndTime >= dateFrom && t.DateAndTime <= dateTo && t.Price >= int.Parse(priceFrom))
                        searchedFor.Add(t);
                }
                HttpContext.Application["searched"] = searchedFor;
                return View("SearchResult", searchedFor);
            }

            if (name == "" && priceFrom == "" && priceTo != "")
            {
                foreach (Ticket t in tickets)
                {
                    if (t.DateAndTime >= dateFrom && t.DateAndTime <= dateTo && t.Price <= int.Parse(priceTo))
                        searchedFor.Add(t);
                }
                HttpContext.Application["searched"] = searchedFor;
                return View("SearchResult", searchedFor);
            }

            if (name != "" && priceFrom != "" && priceTo == "")
            {
                foreach (Ticket t in tickets)
                {
                    if (t.DateAndTime >= dateFrom && t.DateAndTime <= dateTo && t.Price >= int.Parse(priceFrom) && t.ReservedFor.Name.ToLower().Equals(name.ToLower()))
                        searchedFor.Add(t);
                }
                HttpContext.Application["searched"] = searchedFor;
                return View("SearchResult", searchedFor);
            }

            if (name != "" && priceFrom == "" && priceTo != "")
            {
                foreach (Ticket t in tickets)
                {
                    if (t.DateAndTime >= dateFrom && t.DateAndTime <= dateTo && t.Price <= int.Parse(priceTo) && t.ReservedFor.Name.ToLower().Equals(name.ToLower()))
                        searchedFor.Add(t);
                }
                HttpContext.Application["searched"] = searchedFor;
                return View("SearchResult", searchedFor);
            }

            if (name == "" && priceFrom != "" && priceTo != "")
            {
                foreach (Ticket t in tickets)
                {
                    if (t.DateAndTime >= dateFrom && t.DateAndTime <= dateTo && t.Price >= int.Parse(priceFrom) && t.Price <= int.Parse(priceTo))
                        searchedFor.Add(t);
                }
                HttpContext.Application["searched"] = searchedFor;
                return View("SearchResult", searchedFor);
            }

            if (name != "" && priceFrom != "" && priceTo != "")
            {
                foreach (Ticket t in tickets)
                {
                    if (t.ReservedFor.Name.ToLower().Equals(name.ToLower()) && t.DateAndTime >= dateFrom && t.DateAndTime <= dateTo && t.Price >= int.Parse(priceFrom) && t.Price <= int.Parse(priceTo))
                        searchedFor.Add(t);
                }
                HttpContext.Application["searched"] = searchedFor;
                return View("SearchResult", searchedFor);
            }

            return View("ShowTickets", tickets);
        }

        public ActionResult Filter(string vip, string regular, string fanPit, string reserved, string canceled)
        {
            List<Ticket> tickets = (List<Ticket>)HttpContext.Application["myTickets"];
            List<Ticket> filtered = new List<Ticket>();

            if(vip == null && regular == null && fanPit == null && reserved == null && canceled == null)
            {
                return View("ShowTickets", tickets);
            }

            if(vip == "on")
            {
                foreach(Ticket t in tickets)
                {
                    if (t.Type.Equals(TicketType.VIP))
                        filtered.Add(t);
                }
            }
            if (regular == "on")
            {
                foreach (Ticket t in tickets)
                {
                    if (t.Type.Equals(TicketType.REGULAR))
                        filtered.Add(t);
                }
            }
            if (fanPit == "on")
            {
                foreach (Ticket t in tickets)
                {
                    if (t.Type.Equals(TicketType.FAN_PIT))
                        filtered.Add(t);
                }
            }
            if (reserved == "on")
            {
                foreach (Ticket t in tickets)
                {
                    if (t.Status.Equals(TicketStatus.RESERVED))
                        if (!filtered.Contains(t))
                            filtered.Add(t);
                }
            }

            if (canceled == "on")
            {
                foreach (Ticket t in tickets)
                {
                    if (!t.Status.Equals(TicketStatus.RESERVED))
                        if (!filtered.Contains(t))
                            filtered.Add(t);
                }
            }

            return View("ShowTickets", filtered);
        }


        public ActionResult SortTicketsExpensiveFirst()
        {
            List<Ticket> tickets = (List<Ticket>)HttpContext.Application["myTickets"];
            tickets.Sort((x, y) => y.Price.CompareTo(x.Price));
            return View("ShowTickets", tickets);
        }

        public ActionResult SortTicketsCheapFirst()
        {
            List<Ticket> tickets = (List<Ticket>)HttpContext.Application["myTickets"];
            tickets.Sort((x, y) => x.Price.CompareTo(y.Price));
            return View("ShowTickets", tickets);
        }

        public ActionResult SortTicketsNameAsc()
        {
            List<Ticket> tickets = (List<Ticket>)HttpContext.Application["myTickets"];
            tickets.Sort((x, y) => x.ReservedFor.Name.CompareTo(y.ReservedFor.Name));
            return View("ShowTickets", tickets);
        }

        public ActionResult SortTicketsNameDesc()
        {
            List<Ticket> tickets = (List<Ticket>)HttpContext.Application["myTickets"];
            tickets.Sort((x, y) => y.ReservedFor.Name.CompareTo(x.ReservedFor.Name));
            return View("ShowTickets", tickets);
        }

        public ActionResult SortTicketsDateAsc()
        {
            List<Ticket> tickets = (List<Ticket>)HttpContext.Application["myTickets"];
            tickets.Sort((x, y) => x.DateAndTime.CompareTo(y.DateAndTime));
            return View("ShowTickets", tickets);
        }

        public ActionResult SortTicketsDateDesc()
        {
            List<Ticket> tickets = (List<Ticket>)HttpContext.Application["myTickets"];
            tickets.Sort((x, y) => y.DateAndTime.CompareTo(x.DateAndTime));
            return View("ShowTickets", tickets);
        }

        public ActionResult Reset()
        {
            List<Ticket> tickets = (List<Ticket>)HttpContext.Application["originalT"];
            return View("ShowTickets", tickets);
        }

    }
}