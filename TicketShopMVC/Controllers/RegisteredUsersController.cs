using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketShopMVC.Models;

namespace TicketShopMVC.Controllers
{
    public class RegisteredUsersController : Controller
    {
        // GET: RegisteredUsers
        public ActionResult Index()
        {
            List<User> users = (List<User>)HttpContext.Application["users"];
            List<User> showExistingUsers = new List<User>();
            foreach(User user in users)
            {
                if (!user.IsDeleted)
                {
                    showExistingUsers.Add(user);
                }
            }

            return View(showExistingUsers);
        }

        public ActionResult Remove(string username)
        {
            List<User> users = (List<User>)HttpContext.Application["users"];

            for(int i = 0; i < users.Count; ++i)
            {
                if(users[i].Username == username && users[i].IsDeleted.Equals(false))
                {
                    users[i].IsDeleted = true;
                    break;
                }
            }

            FileOperations.OverwriteUsers(users);

            return RedirectToAction("Index", "RegisteredUsers");
        }
        public ActionResult Search(string nameOption, string lastnameOption, string usernameOption)
        {
            List<User> users = (List<User>)HttpContext.Application["users"];
            List<User> searchedFor = new List<User>();

            if(nameOption == null && lastnameOption == null && usernameOption == null)
            {
                return View("Index", users);
            }

            if(nameOption != "" && lastnameOption == "" && usernameOption == "")
            {
                for(int i = 0; i < users.Count; ++i)
                {
                    if (users[i].Name.Equals(nameOption))
                    {
                        searchedFor.Add(users[i]);
                    }
                }
                return View("Index", searchedFor);
            }
            if (nameOption == "" && lastnameOption != "" && usernameOption == "")
            {
                for (int i = 0; i < users.Count; ++i)
                {
                    if (users[i].Lastname.Equals(lastnameOption))
                    {
                        searchedFor.Add(users[i]);
                    }
                }
                return View("Index", searchedFor);
            }
            if (nameOption == "" && lastnameOption == "" && usernameOption != "")
            {
                for (int i = 0; i < users.Count; ++i)
                {
                    if (users[i].Username.Equals(usernameOption))
                    {
                        searchedFor.Add(users[i]);
                    }
                }
                return View("Index", searchedFor);
            }
            if (nameOption != "" && lastnameOption != "" && usernameOption == "")
            {
                for (int i = 0; i < users.Count; ++i)
                {
                    if (users[i].Name.Equals(nameOption) && users[i].Lastname.Equals(lastnameOption))
                    {
                        searchedFor.Add(users[i]);
                    }
                }
                return View("Index", searchedFor);
            }
            if (nameOption != "" && lastnameOption == "" && usernameOption != "")
            {
                for (int i = 0; i < users.Count; ++i)
                {
                    if (users[i].Name.Equals(nameOption) && users[i].Username.Equals(usernameOption))
                    {
                        searchedFor.Add(users[i]);
                    }
                }
                return View("Index", searchedFor);
            }
            if (nameOption == "" && lastnameOption != "" && usernameOption != "")
            {
                for (int i = 0; i < users.Count; ++i)
                {
                    if (users[i].Lastname.Equals(lastnameOption) && users[i].Username.Equals(usernameOption))
                    {
                        searchedFor.Add(users[i]);
                    }
                }
                return View("Index", searchedFor);
            }
            if (nameOption != "" && lastnameOption != "" && usernameOption != "")
            {
                for (int i = 0; i < users.Count; ++i)
                {
                    if (users[i].Name.Equals(nameOption) && users[i].Lastname.Equals(lastnameOption) && users[i].Username.Equals(usernameOption))
                    {
                        searchedFor.Add(users[i]);
                    }
                }
                return View("Index", searchedFor);
            }


            return View("Index", users);
        }

        public ActionResult Filter(string admin, string customer, string salesman, string regular, string bronze, string silver, string gold, string platinum)
        {
            List<User> users = (List<User>)HttpContext.Application["users"];
            List<User> filtered = new List<User>();
            
            if(admin == "on")
            {
                foreach(User u in users)
                {
                    if (u.Role.Equals(Role.ADMINISTRATOR))
                    {
                        filtered.Add(u);
                    }
                }
            }
            if (customer == "on")
            {
                foreach (User u in users)
                {
                    if (u.Role.Equals(Role.CUSTOMER))
                    {
                        filtered.Add(u);
                    }
                }
            }

            if (salesman == "on")
            {
                foreach (User u in users)
                {
                    if (u.Role.Equals(Role.SALESMAN))
                    {
                        filtered.Add(u);
                    }
                }
            }

            if (regular == "on")
            {
                foreach (User u in users)
                {
                    if (u.Role.Equals(Role.CUSTOMER))
                    {
                        if(u.UserType.TypeName.Equals("Regular"))
                            if(!filtered.Contains(u))
                                filtered.Add(u);
                    }
                }
            }

            if (bronze == "on")
            {
                foreach (User u in users)
                {
                    if (u.Role.Equals(Role.CUSTOMER))
                    {
                        if (u.UserType.TypeName.Equals("Bronze"))
                            if (!filtered.Contains(u))
                                filtered.Add(u);
                    }
                }
            }
            if (silver == "on")
            {
                foreach (User u in users)
                {
                    if (u.Role.Equals(Role.CUSTOMER))
                    {
                        if (u.UserType.TypeName.Equals("Silver"))
                            if (!filtered.Contains(u))
                                filtered.Add(u);
                    }
                }
            }
            if (gold == "on")
            {
                foreach (User u in users)
                {
                    if (u.Role.Equals(Role.CUSTOMER))
                    {
                        if (u.UserType.TypeName.Equals("Gold"))
                            if (!filtered.Contains(u))
                                filtered.Add(u);
                    }
                }
            }

            if (platinum == "on")
            {
                foreach (User u in users)
                {
                    if (u.Role.Equals(Role.CUSTOMER))
                    {
                        if (u.UserType.TypeName.Equals("Platinum"))
                            if (!filtered.Contains(u))
                                filtered.Add(u);
                    }
                }
            }
            
            return View("Index", filtered);
        }

        public ActionResult SortNameAsc()
        {
            List<User> users = (List<User>)HttpContext.Application["users"];
            users.Sort((x, y) => x.Name.CompareTo(y.Name));
            return View("Index", users);
        }

        public ActionResult SortNameDesc()
        {
            List<User> users = (List<User>)HttpContext.Application["users"];
            users.Sort((x, y) => y.Name.CompareTo(x.Name));
            return View("Index", users);
        }

        //lastname
        public ActionResult SortLastnameAsc()
        {
            List<User> users = (List<User>)HttpContext.Application["users"];
            users.Sort((x, y) => x.Lastname.CompareTo(y.Lastname));
            return View("Index", users);
        }

        public ActionResult SortLastnameDesc()
        {
            List<User> users = (List<User>)HttpContext.Application["users"];
            users.Sort((x, y) => y.Lastname.CompareTo(x.Lastname));
            return View("Index", users);
        }

        //username
        public ActionResult SortUsernameAsc()
        {
            List<User> users = (List<User>)HttpContext.Application["users"];
            users.Sort((x, y) => x.Username.CompareTo(y.Username));
            return View("Index", users);
        }

        public ActionResult SortUsernameDesc()
        {
            List<User> users = (List<User>)HttpContext.Application["users"];
            users.Sort((x, y) => y.Username.CompareTo(x.Username));
            return View("Index", users);
        }

        //points
        public ActionResult SortPointsAsc()
        {
            List<User> users = (List<User>)HttpContext.Application["users"];
            users.Sort((x, y) => x.EarnedPoints.CompareTo(y.EarnedPoints));
            return View("Index", users);
        }

        public ActionResult SortPointsDesc()
        {
            List<User> users = (List<User>)HttpContext.Application["users"];
            users.Sort((x, y) => y.EarnedPoints.CompareTo(x.EarnedPoints));
            return View("Index", users);
        }

        public ActionResult Reset()
        {
            List<User> users = (List<User>)HttpContext.Application["users"];
            return RedirectToAction("Index", "RegisteredUsers");
        }
    }
}