using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using PizzaWepApi.Models;
using PizzaWepApi.Models.ClientModels;
using PizzaWepApi.Services;

namespace PizzaWepApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AccountController : ApiController
    {
        private PizzaWebApiDbEntities db = new PizzaWebApiDbEntities();

        [HttpPost]
        public HttpResponseMessage Register([FromBody]RegisterInfo client)
        {
            User user = db.User.Where(usr => usr.Nickname == client.Nickname && usr.PasswordHash == client.PasswordHash && usr.Email == client.Email).FirstOrDefault();

            if (user != null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Such user allready exists");

            Guid refreshToken = Guid.NewGuid();
            User registerUser = new User() { Email = client.Email, Nickname = client.Nickname, PasswordHash = client.PasswordHash, RefreshToken = refreshToken, RefreshTokenExpirationDate = DateTime.UtcNow.AddDays(1) };
            db.User.Add(registerUser);
            db.SaveChanges();

            User allreadyRegisteredUser = db.User.Where(usr => usr.Nickname == client.Nickname && usr.PasswordHash == client.PasswordHash && usr.Email == client.Email).FirstOrDefault();

            if (allreadyRegisteredUser == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Error during registration");

            AuthInfo authenticationInfo = new AuthInfo() { UserId = allreadyRegisteredUser.Id, NickName = allreadyRegisteredUser.Nickname, JWT_Token = JWT_Manager.GenerateToken(allreadyRegisteredUser.Id, allreadyRegisteredUser.Nickname, 5), RefreshToken = refreshToken.ToString() };
            return Request.CreateResponse(HttpStatusCode.OK, authenticationInfo);
        }

        [HttpPost]
        public HttpResponseMessage Login([FromBody]LoginInfo loginInfo)
        {
            User user = db.User.Where(usr => usr.Email == loginInfo.Email && usr.PasswordHash == loginInfo.PasswordHash).FirstOrDefault();

            if (user == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Such user does not exist");

            Guid refreshToken = Guid.NewGuid();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpirationDate = DateTime.UtcNow.AddDays(1);
            db.SaveChanges();

            AuthInfo authenticationInfo = new AuthInfo() { UserId = user.Id, NickName = user.Nickname, JWT_Token = JWT_Manager.GenerateToken(user.Id, user.Nickname, 5), RefreshToken = user.RefreshToken.ToString() };
            return Request.CreateResponse(HttpStatusCode.OK, authenticationInfo);
        }

        [HttpPost]
        public HttpResponseMessage RefreshToken([FromBody] RefreshInfo refreshInfo)
        {
            User user = db.User.Where(usr => usr.Id == refreshInfo.Id).FirstOrDefault();

            if (user == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "User was not found");

            Guid UserRefreshToken = user.RefreshToken.GetValueOrDefault();

            if (UserRefreshToken == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "There is not refresh token");

            if(UserRefreshToken.ToString() == refreshInfo.RefreshToken)
            {
                DateTime now = DateTime.UtcNow;
                DateTime RefreshTokenExpirationDate = user.RefreshTokenExpirationDate.GetValueOrDefault();

                if(RefreshTokenExpirationDate > now)
                {
                    //Guid newRefreshToken = Guid.NewGuid();
                    //DateTime newRefreshTokenExpirationDate = DateTime.UtcNow.AddDays(1);
                    //user.RefreshToken = newRefreshToken;
                    //user.RefreshTokenExpirationDate = newRefreshTokenExpirationDate;
                    //db.SaveChanges();

                    AuthInfo authenticationInfo = new AuthInfo() { UserId = user.Id, NickName = user.Nickname, JWT_Token = JWT_Manager.GenerateToken(user.Id, user.Nickname, 5), RefreshToken = UserRefreshToken.ToString() };
                    return Request.CreateResponse(HttpStatusCode.OK, authenticationInfo);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Refresh token expired");
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Wrong refresh token");
            }     
        }
    }
}
