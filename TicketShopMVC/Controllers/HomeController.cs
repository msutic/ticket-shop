using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketShopMVC.Models;

namespace TicketShopMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Manifestation> manifestations = (List<Manifestation>)HttpContext.Application["manifestations"];
            
            return View(manifestations);
        }

        public ActionResult SortManifestationsExpensiveFirst()
        {
            List<Manifestation> manifestations = (List<Manifestation>)HttpContext.Application["manifestations"];

            manifestations.Sort((x, y) => y.PriceRegular.CompareTo(x.PriceRegular));
            
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SortManifestationsCheapFirst()
        {
            List<Manifestation> manifestations = (List<Manifestation>)HttpContext.Application["manifestations"];

            manifestations.Sort((x, y) => x.PriceRegular.CompareTo(y.PriceRegular));
            
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SortManifestationsNameAsc()
        {
            List<Manifestation> manifestations = (List<Manifestation>)HttpContext.Application["manifestations"];

            manifestations.Sort((x, y) => x.Name.CompareTo(y.Name));
            
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SortManifestationsNameDesc()
        {
            List<Manifestation> manifestations = (List<Manifestation>)HttpContext.Application["manifestations"];

            manifestations.Sort((x, y) => y.Name.CompareTo(x.Name));
            
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SortManifestationsDateAsc()
        {
            List<Manifestation> manifestations = (List<Manifestation>)HttpContext.Application["manifestations"];

            manifestations.Sort((x, y) => x.DateAndTime.CompareTo(y.DateAndTime));

            return RedirectToAction("Index", "Home");
        }

        public ActionResult SortManifestationsDateDesc()
        {
            List<Manifestation> manifestations = (List<Manifestation>)HttpContext.Application["manifestations"];

            manifestations.Sort((x, y) => y.DateAndTime.CompareTo(x.DateAndTime));

            return RedirectToAction("Index", "Home");
        }

        public ActionResult SortManifestationsPlaceAsc()
        {
            List<Manifestation> manifestations = (List<Manifestation>)HttpContext.Application["manifestations"];

            manifestations.Sort((x, y) => x.Place.City.CompareTo(y.Place.City));

            return RedirectToAction("Index", "Home");
        }

        public ActionResult SortManifestationsPlaceDesc()
        {
            List<Manifestation> manifestations = (List<Manifestation>)HttpContext.Application["manifestations"];

            manifestations.Sort((x, y) => y.Place.City.CompareTo(x.Place.City));

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ResetSort()
        {
            List<Manifestation> manifestations = (List<Manifestation>)HttpContext.Application["manifestations"];
            manifestations.Sort((x, y) => x.DateAndTime.CompareTo(y.DateAndTime));
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Search(string name, string city, DateTime dateFrom, DateTime dateTo, string priceFrom, string priceTo)
        {
            List<Manifestation> manifestations = (List<Manifestation>)HttpContext.Application["manifestations"];
            List<Manifestation> searchedFor = new List<Manifestation>();

            if(name == "" && city=="" && priceFrom == "" && priceTo == "")
            {
                foreach(Manifestation m in manifestations)
                {
                    if (m.DateAndTime >= dateFrom && m.DateAndTime <= dateTo)
                        searchedFor.Add(m);
                }
                HttpContext.Application["searched"] = searchedFor;
                return View("SearchResult", searchedFor);
            }
            
            if (name != "" && city == "" && priceFrom == "" && priceTo == "") 
            {
                foreach(Manifestation m in manifestations)
                {
                    if (m.Name.ToLower().Equals(name.ToLower()) && m.DateAndTime >= dateFrom && m.DateAndTime <= dateTo)
                        searchedFor.Add(m);
                }
                ViewBag.Message = $"Searched: {name}";
                HttpContext.Application["searched"] = searchedFor;
                return View("SearchResult", searchedFor);
            }

            if(name == "" && city != "" && priceFrom == "" && priceTo == "")
            {
                foreach (Manifestation m in manifestations)
                {
                    if (m.Place.City.ToLower().Equals(city.ToLower()) && m.DateAndTime >= dateFrom && m.DateAndTime <= dateTo)
                        searchedFor.Add(m);
                }
                ViewBag.Message = $"Searched: {city}";
                HttpContext.Application["searched"] = searchedFor;
                return View("SearchResult", searchedFor);
            }

            if (name == "" && city == "" && priceFrom != "" && priceTo == "")
            {
                foreach (Manifestation m in manifestations)
                {
                    if (m.PriceRegular >= int.Parse(priceFrom) && m.DateAndTime >= dateFrom && m.DateAndTime <= dateTo)
                        searchedFor.Add(m);
                }
                ViewBag.Message = $"Searched: Price from {priceFrom}";
                HttpContext.Application["searched"] = searchedFor;
                return View("SearchResult", searchedFor);
            }

            if (name == "" && city == "" && priceFrom == "" && priceTo != "")
            {
                foreach (Manifestation m in manifestations)
                {
                    if (m.PriceRegular <= int.Parse(priceTo) && m.DateAndTime >= dateFrom && m.DateAndTime <= dateTo)
                        searchedFor.Add(m);
                }
                ViewBag.Message = $"Searched: Price to {priceTo}";
                HttpContext.Application["searched"] = searchedFor;
                return View("SearchResult", searchedFor);
            }

            if (name != "" && city != "" && priceFrom == "" && priceTo == "")
            {
                foreach (Manifestation m in manifestations)
                {
                    if (m.Name.ToLower().Equals(name.ToLower()) && m.Place.City.ToLower().Equals(city.ToLower()))
                        if(m.DateAndTime >= dateFrom && m.DateAndTime <= dateTo)
                            searchedFor.Add(m);
                }
                ViewBag.Message = $"Searched: {name}, {city}";
                HttpContext.Application["searched"] = searchedFor;
                return View("SearchResult", searchedFor);
            }

            if (name != "" && city == "" && priceFrom != "" && priceTo == "")
            {
                foreach (Manifestation m in manifestations)
                {
                    if (m.Name.ToLower().Equals(name.ToLower()) && m.PriceRegular >= int.Parse(priceFrom))
                        if(m.DateAndTime >= dateFrom && m.DateAndTime <= dateTo)
                            searchedFor.Add(m);
                }
                ViewBag.Message = $"Searched: {name}, Price from {priceFrom}";
                HttpContext.Application["searched"] = searchedFor;
                return View("SearchResult", searchedFor);
            }

            if (name != "" && city == "" && priceFrom == "" && priceTo != "")
            {
                foreach (Manifestation m in manifestations)
                {
                    if (m.Name.ToLower().Equals(name.ToLower()) && m.PriceRegular <= int.Parse(priceTo))
                        if (m.DateAndTime >= dateFrom && m.DateAndTime <= dateTo)
                            searchedFor.Add(m);
                }
                ViewBag.Message = $"Searched: {name}, Price to {priceTo}";
                HttpContext.Application["searched"] = searchedFor;
                return View("SearchResult", searchedFor);
            }

            if (name == "" && city != "" && priceFrom != "" && priceTo == "")
            {
                foreach (Manifestation m in manifestations)
                {
                    if (m.Place.City.ToLower().Equals(city.ToLower()) && m.PriceRegular >= int.Parse(priceFrom))
                        if (m.DateAndTime >= dateFrom && m.DateAndTime <= dateTo)
                            searchedFor.Add(m);
                }
                ViewBag.Message = $"Searched: {city}, Price from {priceFrom}";
                HttpContext.Application["searched"] = searchedFor;
                return View("SearchResult", searchedFor);
            }

            if (name == "" && city != "" && priceFrom == "" && priceTo != "")
            {
                foreach (Manifestation m in manifestations)
                {
                    if (m.Place.City.ToLower().Equals(city.ToLower()) && m.PriceRegular <= int.Parse(priceTo))
                        if (m.DateAndTime >= dateFrom && m.DateAndTime <= dateTo)
                            searchedFor.Add(m);
                }
                HttpContext.Application["searched"] = searchedFor;
                ViewBag.Message = $"Searched: {city}, Price to {priceTo}";
                return View("SearchResult", searchedFor);
            }

            if (name == "" && city == "" && priceFrom != "" && priceTo != "")
            {
                foreach (Manifestation m in manifestations)
                {
                    if (m.PriceRegular <= int.Parse(priceTo) && m.PriceRegular >= int.Parse(priceFrom))
                        if (m.DateAndTime >= dateFrom && m.DateAndTime <= dateTo)
                            searchedFor.Add(m);
                }
                ViewBag.Message = $"Searched: Price from {priceFrom} to {priceTo}";
                HttpContext.Application["searched"] = searchedFor;
                return View("SearchResult", searchedFor);
            }

            if (name != "" && city != "" && priceFrom != "" && priceTo == "")
            {
                foreach (Manifestation m in manifestations)
                {
                    if (m.Place.City.ToLower().Equals(city.ToLower()) && m.DateAndTime >= dateFrom && m.DateAndTime <= dateTo && m.Name.ToLower().Equals(name.ToLower()) && m.PriceRegular >= int.Parse(priceFrom))
                        searchedFor.Add(m);
                }
                ViewBag.Message = $"Searched: {name}, {city}, Price from {priceFrom}";
                HttpContext.Application["searched"] = searchedFor;
                return View("SearchResult", searchedFor);
            }

            if (name != "" && city != "" && priceFrom == "" && priceTo != "")
            {
                foreach (Manifestation m in manifestations)
                {
                    if (m.Place.City.ToLower().Equals(city.ToLower()) && m.DateAndTime >= dateFrom && m.DateAndTime <= dateTo && m.Name.ToLower().Equals(name.ToLower()) && m.PriceRegular <= int.Parse(priceTo))
                        searchedFor.Add(m);
                }
                ViewBag.Message = $"Searched: {name}, {city}, Price to {priceTo}";
                HttpContext.Application["searched"] = searchedFor;
                return View("SearchResult", searchedFor);
            }

            if (name != "" && city == "" && priceFrom != "" && priceTo != "")
            {
                foreach (Manifestation m in manifestations)
                {
                    if (m.DateAndTime >= dateFrom && m.DateAndTime <= dateTo && m.Name.ToLower().Equals(name.ToLower()) && m.PriceRegular <= int.Parse(priceTo) && m.PriceRegular >= int.Parse(priceFrom))
                        searchedFor.Add(m);
                }
                ViewBag.Message = $"Searched: {name}, Price from {priceFrom} to {priceTo}";
                HttpContext.Application["searched"] = searchedFor;

                return View("SearchResult", searchedFor);
            }

            if (name == "" && city != "" && priceFrom != "" && priceTo != "")
            {
                foreach (Manifestation m in manifestations)
                {
                    if (m.DateAndTime >= dateFrom && m.DateAndTime <= dateTo && m.Place.City.ToLower().Equals(city.ToLower()) && m.PriceRegular <= int.Parse(priceTo) && m.PriceRegular >= int.Parse(priceFrom))
                        searchedFor.Add(m);
                }
                ViewBag.Message = $"Searched: {city}, Price from {priceFrom} to {priceTo}";
                HttpContext.Application["searched"] = searchedFor;
                return View("SearchResult", searchedFor);
            }

            if (name != "" && city != "" && priceFrom != "" && priceTo != "")
            {
                foreach (Manifestation m in manifestations)
                {
                    if (m.Name.ToLower().Equals(name.ToLower()) && m.DateAndTime >= dateFrom && m.DateAndTime <= dateTo && m.Place.City.ToLower().Equals(city.ToLower()) && m.PriceRegular <= int.Parse(priceTo) && m.PriceRegular >= int.Parse(priceFrom))
                        searchedFor.Add(m);
                }
                ViewBag.Message = $"Searched: {name},{city}, Price from {priceFrom} to {priceTo}";
                HttpContext.Application["searched"] = searchedFor;
                return View("SearchResult", searchedFor);
            }

            return View("Index", manifestations);
        }

        public ActionResult SearchResult()
        {
            return View();
        }

        public ActionResult Filter(string koncert, string festa, string festival, string available)
        {
            List<Manifestation> manifestations = (List<Manifestation>)HttpContext.Application["manifestations"];
            List<Manifestation> filtered = new List<Manifestation>();

            if (koncert == null && festa == null && festival == null && available == null)
                return View("Index", manifestations);
            
            if (koncert == "on")
            {
                foreach(Manifestation m in manifestations)
                {
                    if (m.ManifestationType.Equals("Koncert"))
                        filtered.Add(m);
                }
            }

            if (festa == "on")
            {
                foreach(Manifestation m in manifestations)
                {
                    if (m.ManifestationType.Equals("Festa"))
                        filtered.Add(m);
                }
            }

            if (festival == "on")
            {
                foreach (Manifestation m in manifestations)
                {
                    if (m.ManifestationType.Equals("Festival"))
                        filtered.Add(m);
                }
            }

            if(available == "on")
            {
                foreach(Manifestation m in manifestations)
                {
                    if (m.seatsAvailable > 0)
                        filtered.Add(m);
                }
            }
            

            return View("Index", filtered);
        }

        public ActionResult FilterSearched(string koncert, string festa, string festival, string available)
        {
            List<Manifestation> searched = (List<Manifestation>)HttpContext.Application["searched"];
            List<Manifestation> filtered = new List<Manifestation>();

            if (koncert == null && festa == null && festival == null && available == null)
                return View("Index", searched);

            if (koncert == "on")
            {
                foreach (Manifestation m in searched)
                {
                    if (m.ManifestationType.Equals("Koncert"))
                        filtered.Add(m);
                }
            }

            if (festa == "on")
            {
                foreach (Manifestation m in searched)
                {
                    if (m.ManifestationType.Equals("Festa"))
                        filtered.Add(m);
                }
            }

            if (festival == "on")
            {
                foreach (Manifestation m in searched)
                {
                    if (m.ManifestationType.Equals("Festival"))
                        filtered.Add(m);
                }
            }

            return View("SearchResult", filtered);
        }

        //SORTING FOR SEARCHED
        //HEYYYY
        //
        //
        //
        //
        //
        //
        public ActionResult SortManifestationsExpensiveFirstSearched()
        {
            List<Manifestation> searched = (List<Manifestation>)HttpContext.Application["searched"];

            searched.Sort((x, y) => y.PriceRegular.CompareTo(x.PriceRegular));

            return View("SearchResult",searched);
        }

        public ActionResult SortManifestationsCheapFirstSearched()
        {
            List<Manifestation> searched = (List<Manifestation>)HttpContext.Application["searched"];

            searched.Sort((x, y) => x.PriceRegular.CompareTo(y.PriceRegular));

            return View("SearchResult", searched);
        }

        public ActionResult SortManifestationsNameAscSearched()
        {
            List<Manifestation> searched = (List<Manifestation>)HttpContext.Application["searched"];

            searched.Sort((x, y) => x.Name.CompareTo(y.Name));

            return View("SearchResult", searched);
        }

        public ActionResult SortManifestationsNameDescSearched()
        {
            List<Manifestation> searched = (List<Manifestation>)HttpContext.Application["searched"];

            searched.Sort((x, y) => y.Name.CompareTo(x.Name));

            return View("SearchResult", searched);
        }

        public ActionResult SortManifestationsDateAscSearched()
        {
            List<Manifestation> searched = (List<Manifestation>)HttpContext.Application["searched"];

            searched.Sort((x, y) => x.DateAndTime.CompareTo(y.DateAndTime));

            return View("SearchResult", searched);
        }

        public ActionResult SortManifestationsDateDescSearched()
        {
            List<Manifestation> searched = (List<Manifestation>)HttpContext.Application["searched"];

            searched.Sort((x, y) => y.DateAndTime.CompareTo(x.DateAndTime));

            return View("SearchResult", searched);
        }

        public ActionResult SortManifestationsPlaceAscSearched()
        {
            List<Manifestation> searched = (List<Manifestation>)HttpContext.Application["searched"];

            searched.Sort((x, y) => x.Place.City.CompareTo(y.Place.City));

            return View("SearchResult", searched);
        }

        public ActionResult SortManifestationsPlaceDescSearched()
        {
            List<Manifestation> searched = (List<Manifestation>)HttpContext.Application["searched"];

            searched.Sort((x, y) => y.Place.City.CompareTo(x.Place.City));

            return View("SearchResult", searched);
        }

    }
}