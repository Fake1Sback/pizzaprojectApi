using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWepApi.Models.ClientModels
{
    public class LoginInfo
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}