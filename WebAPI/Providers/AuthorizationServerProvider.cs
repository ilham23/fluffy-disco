using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Model.Models;
using Service;
using WebAPI.Infrastructure.Core;
using WebAPI.Models;

namespace WebAPI.Providers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public AuthorizationServerProvider()
        {
        }
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            await Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            UserManager<AspNetUser> userManager = context.OwinContext.GetUserManager<UserManager<AspNetUser>>();
            AspNetUser user;
            try
            {
                user = await userManager.FindAsync(context.UserName, context.Password);
            }
            catch
            {
                // Could not retrieve the user due to error.
                context.SetError("server_error", "Lỗi trong quá trình xử lý.");
                context.Rejected();
                return;
            }
            if (user != null)
            {
                var roles = userManager.GetRoles(user.Id);
                ClaimsIdentity identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ExternalBearer);
                string email = string.IsNullOrEmpty(user.Email) ? "" : user.Email;
                identity.AddClaim(new Claim("fullName", user.Name));
                identity.AddClaim(new Claim("email", email));
                identity.AddClaim(new Claim("username", user.UserName));
                identity.AddClaim(new Claim("roles", JsonConvert.SerializeObject(roles)));
                var props = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        {"fullName", user.Name},
                        {"email", email},
                        {"username", user.UserName},
                        {"roles",JsonConvert.SerializeObject(roles) }

                    });
                context.Validated(new AuthenticationTicket(identity, props));
            }
            else
            {
                context.SetError("invalid_grant", "Wrong Username or Pasword .");
                context.Rejected();
            }
        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }

    }
}