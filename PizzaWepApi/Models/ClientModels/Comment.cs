using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWepApi.Models.ClientModels
{
    public class Comment
    {
        public string NickName { get; set; }
        public string CommentDate { get; set; }
        public string CommentMessage { get; set; }
    }
}