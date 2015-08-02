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
            var properties = new AuthenticationProperties();
            string apiKey = Request.Headers.Get("ApiKey");
            if (apiKey != "123456")
            {
                return Task.FromResult(new AuthenticationTicket(
                    null, properties));
            }
            else
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new EnterpriseIdentity
                    {
                        Name = "Dispatcher"
                    });
                return Task.FromResult(new AuthenticationTicket(
                    identity, properties));
            }
        }
    }
}
