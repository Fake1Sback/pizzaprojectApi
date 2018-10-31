using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWepApi.Models.ClientModels
{
    public class Card
    {
        public string Title { get; set; }
        public int Cols { get; set; }
        public int Rows { get; set; }
        public string ImageURL { get; set; }
        public string[] AdditionalImagesURL { get; set; }
        public string Content { get; set; }
    }
}