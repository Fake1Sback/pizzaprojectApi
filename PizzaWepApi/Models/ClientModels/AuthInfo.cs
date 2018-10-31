using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWepApi.Models.ClientModels
{
    public class AuthInfo
    {
        public int UserId { get; set; }
        public string NickName { get; set; }
        public string JWT_Token { get; set; }
        public string RefreshToken { get; set; }
    }
}