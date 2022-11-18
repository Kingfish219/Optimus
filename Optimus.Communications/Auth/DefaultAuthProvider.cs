
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;

namespace Optimus.Communications.Auth
{
    public class DefaultAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly IUserAuthManager _authManager;

        public DefaultAuthProvider(IUserAuthManager authManager)
        {
            _authManager = authManager;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            await Task.Run(() =>
            {
                context.Validated();
            });
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var validation = await _authManager.SignIn(context.UserName, context.Password);
            if (validation)
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("Username", context.UserName));
                identity.AddClaim(new Claim("Password", context.Password));
                context.Validated(identity);
            }
            else
            {
                context.SetError("invalid_grant", "The username or password is incorrect.");
            }
        }
    }
}
