using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketShopMVC.Models;

namespace TicketShopMVC.Controllers
{
    public class SalesmenController : Controller
    {
        // GET: Salesmen
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(User user)
        {
            User u = new User();
            List<User> users = (List<User>)HttpContext.Application["users"];

            if (user.Username == null)
            {
                ViewBag.Message = "Username is required.";
                return View("Register");
            }

            for (int i = 0; i < users.Count; ++i)
            {
                if (users[i].Username.Equals(user.Username) && users[i].IsDeleted.Equals(false))
                {
                    ViewBag.Message = $"Username '{user.Username}' is already taken!";
                    return View("Register");
                }
            }
            if (user.Password == null)
            {
                ViewBag.Message = "Password is required.";
                return View("Register");
            }
            if (user.Name == null)
            {
                ViewBag.Message = "Name is required.";
                return View("Register");
            }
            if (user.Lastname == null)
            {
                ViewBag.Message = "Lastname is required.";
                return View("Register");
            }
            if (!user.Gender.Equals(Gender.FEMALE) && !user.Gender.Equals(Gender.MALE) &&
                !user.Gender.Equals(Gender.OTHER))
            {
                ViewBag.Message = "Gender is required.";
                return View("Register");
            }
            if (user.DateOfBirth.Equals(new DateTime(01, 01, 0001)))
            {
                ViewBag.Message = "Date of Birth is required.";
                return View("Register");
            }

            user.Role = Role.SALESMAN;

            users.Add(user);
            FileOperations.SaveUser(user);

            return RedirectToAction("Index","Home");
        }


    }
}