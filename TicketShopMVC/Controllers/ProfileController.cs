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
            for(int i = 0; i < users.Count; i++)
            {
                if (users[i].Username.Equals(username))
                {
                    user = users[i];
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
            User u = (User)HttpContext.Application["userCurrent"];
            user.DateOfBirth = u.DateOfBirth;
            user.Role = u.Role;

            if (user.Username == null)
            {
                TempData["Message"] = "Username is required";
                return RedirectToAction("MiddleAction", "Profile");
            }
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

            List<User> users = (List<User>)HttpContext.Application["users"];
            
            if (user.Role.Equals(Role.CUSTOMER))
            {
                user.EarnedPoints = u.EarnedPoints;
                user.UserType = u.UserType;
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

    }
}