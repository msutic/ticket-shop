using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketShopMVC.Models
{
    public class Poster
    {
        public string Img { get; set; }
        public string ImgPath { get; set; }

        public Poster(string img, string imgPath)
        {
            Img = img;
            ImgPath = imgPath;
        }
    }
}