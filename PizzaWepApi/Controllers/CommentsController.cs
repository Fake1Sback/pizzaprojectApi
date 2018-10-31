using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using PizzaWepApi.Models.ClientModels;
using PizzaWepApi.Models;
using PizzaWepApi.Filters;

namespace PizzaWepApi.Controllers
{
    [EnableCors(origins:"*",headers:"*",methods:"*")]
    public class CommentsController : ApiController
    {
        private PizzaWebApiDbEntities db = new PizzaWebApiDbEntities();
        [HttpGet]
        public HttpResponseMessage GetComments()
        {
            List<Comments> dbComments = db.Comments.ToList();
            List<Comment> commentsToSend = new List<Comment>();
            foreach(Comments com in dbComments)
            {
                Comment comment = new Comment() { NickName = com.User.Nickname, CommentDate = com.CommentDate.ToString("dd/MM/yyyy HH:mm"), CommentMessage = com.CommentMessage };
                commentsToSend.Add(comment);
            }
            return Request.CreateResponse(HttpStatusCode.OK, commentsToSend);
        }

        [HttpPost]
        [JwtAuthentication]
        public HttpResponseMessage PostComment([FromBody]PostComment comment)
        {
            User user = db.User.Where(usr => usr.Id == comment.Id).FirstOrDefault();

            if (user == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "User with such id does not exist");

            Comments dbcomment = new Comments();
            dbcomment.AuthorId = comment.Id;
            dbcomment.CommentMessage = comment.CommentMessage;
            dbcomment.CommentDate = DateTime.Now;
            db.Comments.Add(dbcomment);
            db.SaveChanges();

            List<Comments> dbComments = db.Comments.ToList();
            List<Comment> commentsToSend = new List<Comment>();
            foreach (Comments com in dbComments)
            {
                Comment commentToAdd = new Comment() { NickName = com.User.Nickname, CommentDate = com.CommentDate.ToString("dd/MM/yyyy HH:mm"), CommentMessage = com.CommentMessage };
                commentsToSend.Add(commentToAdd);
            }
            return Request.CreateResponse(HttpStatusCode.OK, commentsToSend);
        }
    }
}
