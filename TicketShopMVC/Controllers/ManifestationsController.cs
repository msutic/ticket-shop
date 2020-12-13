using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketShopMVC.Models;

namespace TicketShopMVC.Controllers
{
    public class ManifestationsController : Controller
    {
        // GET: Manifestations
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Manifestation manifestation, string address, string city, int zipCode)
        {
            User user = (User)Session["User"];

            List<Manifestation> manifestations = (List<Manifestation>)HttpContext.Application["manifestations"];
            Place place = new Place(address, city, zipCode);
            manifestation.Place = place;

            if(manifestation.Name == null)
            {
                ViewBag.Message = "Manifestation name is required.";
                return View("Index");
            }
            for(int i = 0; i < manifestations.Count; i++)
            {
                if (manifestations[i].Name.Equals(manifestation.Name))
                {
                    ViewBag.Message = $"Manifestation with name {manifestation.Name} already exists.";
                    return View("Index");
                }
            }
            if(manifestation.ManifestationType == null)
            {
                ViewBag.Message = "Manifestation kind is required.";
                return View("Index");
            }
            if(manifestation.numOfSeats <= 0)
            {
                ViewBag.Message = "Capacity must be atleast 1.";
                return View("Index");
            }
            if(manifestation.DateAndTime.Equals(new DateTime(01, 01, 0001)))
            {
                ViewBag.Message = "Date is required.";
                return View("Index");
            }
            if (manifestation.PriceRegular < 0)
            {
                ViewBag.Message = "Price can not be negative.";
                return View("Index");
            }
            if (manifestation.Place.Address == null || address.Equals(""))
            {
                ViewBag.Message = "Street and number are required.";
                return View("Index");
            }
            if(manifestation.Place.City == null || city.Equals(""))
            {
                ViewBag.Message = "City is required.";
                return View("Index");
            }
            if (manifestation.Place.ZIPCode <0)
            {
                ViewBag.Message = "ZIP code cannot be negative.";
                return View("Index");
            }

            for (int i = 0; i < manifestations.Count; i++)
            {
                if (manifestations[i].Place.Address.ToLower().Equals(manifestation.Place.Address.ToLower()) && manifestations[i].Place.City.ToLower().Equals(manifestation.Place.City.ToLower()) &&
                    manifestations[i].Place.ZIPCode == manifestation.Place.ZIPCode)
                {
                    if (manifestations[i].DateAndTime == manifestation.DateAndTime)
                    {
                        ViewBag.Message = $"Manifestation cannot be held at the same time with other manifestations.";
                        return View("Index");
                    }
                }
            }

            manifestation.Status = Status.INACTIVE;
            manifestations.Add(manifestation);
            FileOperations.SaveManifestation(manifestation);
            List<User> users = (List<User>)HttpContext.Application["users"];
            
            for(int i = 0; i < users.Count; i++)
            {
                if (users[i].Username.Equals(user.Username))
                {
                    user.Manifestations.Add(manifestation);
                    break;
                }
            }
            FileOperations.OverwriteUsers(users);

            manifestations.Sort((x, y) => x.DateAndTime.CompareTo(y.DateAndTime));

            

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Allow(string id)
        {
            List<Manifestation> manifestations = (List<Manifestation>)HttpContext.Application["manifestations"];

            for(int i = 0; i < manifestations.Count; ++i)
            {
                if(i == int.Parse(id))
                {
                    manifestations[i].Status = Status.ACTIVE;
                    break;
                }
            }
            FileOperations.OverwriteManifestations(manifestations);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Info(string id)
        {
            List<Manifestation> manifestations = (List<Manifestation>)HttpContext.Application["manifestations"];
            Manifestation m = new Manifestation();
            Index index = new Index();
            for(int i =0; i < manifestations.Count; i++)
            {
                if(i == int.Parse(id))
                {
                    m = manifestations[i];
                    index.SavedIndex = i;
                    HttpContext.Application["savedIndex"] = index; 
                    HttpContext.Application["selectedManifestation"] = m;
                    break;
                }
            }

            return View(m);
        }

        public ActionResult Edit(string name)
        {
            List<Manifestation> manifestations = (List<Manifestation>)HttpContext.Application["manifestations"];
            Manifestation manifest = new Manifestation();

            Manifestation selectedManifestation = (Manifestation)HttpContext.Application["selectedManifestation"];

            return View(selectedManifestation);
        }

        public ActionResult MiddleAction()
        {
            Manifestation selectedManifestation = (Manifestation)HttpContext.Application["selectedManifestation"];
            ViewBag.Message = (string)TempData["Message"];

            return View(selectedManifestation);
        }

        [HttpPost]
        public ActionResult Modify(Manifestation manifestation, string address, string city, string zip, string state)
        {
            List<Manifestation> allManifestations = (List<Manifestation>)HttpContext.Application["manifestations"];
            Manifestation selectedManifestation = (Manifestation)HttpContext.Application["selectedManifestation"];

            Index savedIndex = (Index)HttpContext.Application["savedIndex"];


            Place place = new Place(address,city,int.Parse(zip));
            manifestation.Place = place;
            manifestation.Status = (Status)Enum.Parse(typeof(Status), state);

            if(!manifestation.Status.Equals(Status.ACTIVE) && !manifestation.Status.Equals(Status.INACTIVE))
            {
                TempData["Message"] = "Manifestation status is required.";
                return RedirectToAction("MiddleAction", "Manifestations");
            }

            if (selectedManifestation.Status.Equals(Status.INACTIVE))
            {
                if (manifestation.Status.Equals(Status.ACTIVE))
                {
                    TempData["Message"] = "You don't have the authority to change manifestation state from INACTIVE to ACTIVE.";
                    return RedirectToAction("MiddleAction", "Manifestations");
                }
            }

            if (manifestation.Name == null)
            {
                TempData["Message"] = "Manifestation name is required.";
                return RedirectToAction("MiddleAction","Manifestations");
            }
            for (int i = 0; i < allManifestations.Count; i++)
            {
                if (allManifestations[i].Name.Equals(manifestation.Name))
                {
                    if(savedIndex.SavedIndex != i)
                    {
                        TempData["Message"] = $"Manifestation with name {manifestation.Name} already exists.";
                        return RedirectToAction("MiddleAction", "Manifestations");
                    }
                    
                }
            }
            if (manifestation.ManifestationType == null)
            {
                TempData["Message"] = "Manifestation name is required.";
                return RedirectToAction("MiddleAction", "Manifestations");
            }
            if (manifestation.numOfSeats <= 0)
            {
                TempData["Message"] = "Capacity must be atleast 1.";
                return RedirectToAction("MiddleAction", "Manifestations");
            }
            if (manifestation.DateAndTime.Equals(new DateTime(01, 01, 0001)))
            {
                TempData["Message"] = "Date is required.";
                return RedirectToAction("MiddleAction", "Manifestations");
            }

            if (manifestation.PriceRegular < 0)
            {
                TempData["Message"] = "Price can not be negative.";
                return RedirectToAction("MiddleAction", "Manifestations");
            }
            if (manifestation.Place.Address == null)
            {
                TempData["Message"] = "Street and number are required.";
                return RedirectToAction("MiddleAction", "Manifestations");
            }
            if (manifestation.Place.City == null)
            {
                TempData["Message"] = "City is required.";
                return RedirectToAction("MiddleAction", "Manifestations");
            }
            if (manifestation.Place.ZIPCode < 0)
            {
                TempData["Message"] = "ZIP code cannot be negative.";
                return RedirectToAction("MiddleAction", "Manifestations");
            }

            for(int i = 0; i < allManifestations.Count; i++)
            {
                if (allManifestations[i].Place.Address.ToLower().Equals(manifestation.Place.Address.ToLower()) && allManifestations[i].Place.City.ToLower().Equals(manifestation.Place.City.ToLower()) &&
                    allManifestations[i].Place.ZIPCode == manifestation.Place.ZIPCode)
                {
                    if(i != savedIndex.SavedIndex)
                    {
                        if(allManifestations[i].DateAndTime == manifestation.DateAndTime)
                        {
                            TempData["Message"] = $"Manifestation cannot be held at the same time with other manifestations.";
                            return RedirectToAction("MiddleAction", "Manifestations");
                        }
                    }
                }
            }
            
            for (int i=0; i < allManifestations.Count; i++)
            {
                if(savedIndex.SavedIndex == i)
                {
                    allManifestations[i] = manifestation;
                }
            }

            FileOperations.OverwriteManifestations(allManifestations);

            User user = (User)Session["User"];
            List<User> users = (List<User>)HttpContext.Application["users"];

            for(int i = 0; i < user.Manifestations.Count; i++)
            {
                if (user.Manifestations[i].Name.ToLower().Equals(selectedManifestation.Name.ToLower()))
                {
                    user.Manifestations[i] = manifestation;
                    break;
                }
            }

            for(int i = 0; i < users.Count; i++)
            {
                if (users[i].Username.Equals(user.Username))
                {
                    users[i] = user;
                }
            }

            FileOperations.OverwriteUsers(users);

            //for(int i = 0; i < users.Count; i++)
            //{
            //    if(users[i].Manifestations.Count != 0)
            //    {
            //        List<Manifestation> mani = users[i].Manifestations;
            //        for(int j = 0; j < mani.Count; j++)
            //        {
            //            if(mani[j].Name == selectedManifestation.Name)
            //            {
            //                mani[j] = manifestation;
            //                break;
            //            }
            //        }
            //        users[i].Manifestations = mani;
            //    }
            //}


            return View("Info",manifestation);
        }
    }
}