using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketShopMVC.Models;

namespace TicketShopMVC.Controllers
{
    public class AuthenticateController : Controller
    {
        // GET: Authenticate
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            List<User> users = (List<User>)HttpContext.Application["Users"];
            User user = users.Find(u => u.Username.Equals(username) && u.Password.Equals(password) && u.IsDeleted.Equals(false));

            if(user == null)
            {
                ViewBag.Message = "Wrong username or password!";
                return View("Index");
            }

            Session["user"] = user;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            User user = new User();
            return View(user);
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            List<User> users = (List<User>)HttpContext.Application["Users"];
            if(user.Username == null)
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

            users.Add(user);
            FileOperations.SaveUser(user);
            return RedirectToAction("Index", "Authenticate");
        }
    }
}