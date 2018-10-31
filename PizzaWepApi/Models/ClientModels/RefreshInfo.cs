using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWepApi.Models.ClientModels
{
    public class RefreshInfo
    {
        public int Id { get; set; }
        public string RefreshToken { get; set; }
    }
}