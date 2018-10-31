using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWepApi.Models.ClientModels
{
    public class PostComment
    {
        public int Id { get; set; }
        public string CommentMessage { get; set; }
    }
}