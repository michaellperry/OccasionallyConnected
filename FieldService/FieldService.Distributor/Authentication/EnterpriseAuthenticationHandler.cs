using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FieldService.Distributor.Authentication
{
    public class EnterpriseAuthenticationHandler :
        AuthenticationHandler<EnterpriseAuthenticationOptions>
    {
        protected override Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            string apiKey = Request.Headers.Get("ApiKey");
            ClaimsIdentity identity = new ClaimsIdentity(new EnterpriseIdentity
            {
                Name = "Dispatcher"
            });
            var properties = new AuthenticationProperties();
            return Task.FromResult(new AuthenticationTicket(
                identity, properties));
        }
    }
}
