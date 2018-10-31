using PizzaWepApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace PizzaWepApi.Filters
{
    public class JwtAuthenticationAttribute : Attribute,IAuthenticationFilter
    {
        public string Realm { get; set; }
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Bearer")
                return;

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Jwt Token", request);
                return;
            }

            var token = authorization.Parameter;
            var principal = await AuthenticateJwtToken(token);

            if (principal == null)
                context.ErrorResult = new AuthenticationFailureResult("Invalid Token", request);
            else
                context.Principal = principal;
        }

        protected Task<IPrincipal> AuthenticateJwtToken(string token)
        {
            string username;
            int id;

            if (ValidateToken(token, out username, out id))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,username),
                    new Claim(ClaimTypes.NameIdentifier,id.ToString())
                };

                var identity = new ClaimsIdentity(claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);

                return Task.FromResult(user);
            }
            return Task.FromResult<IPrincipal>(null);
        }

        private static bool ValidateToken(string token, out string username,out int id)
        {
            username = null;
            id = 0;
            var simplePrinciple = JWT_Manager.GetPrincipal(token);
            var identity = simplePrinciple?.Identity as ClaimsIdentity;

            if (identity == null)
                return false;

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            var idClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
            username = usernameClaim?.Value;
            id = Convert.ToInt32(idClaim?.Value);

            if (string.IsNullOrEmpty(username))
                return false;

            return true;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parametr = null;

            if (string.IsNullOrEmpty(Realm))
                parametr = "realm\"" + Realm + "\"";

            context.ChallengeWith("Bearer", parametr);
        }

    }
}