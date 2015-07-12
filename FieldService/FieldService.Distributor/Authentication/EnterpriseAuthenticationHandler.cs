using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Security.Principal;
namespace FieldService.Distributor.Authentication
{
    public class EnterpriseAuthenticationHandler :
        AuthenticationHandler<EnterpriseAuthenticationOptions>
    {
        protected override Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            ClaimsIdentity identity = new ClaimsIdentity(new EnterpriseIdentity
            {
                Name = "Dispatcher"
            });
            AuthenticationProperties properties = null;
            return Task.FromResult(new AuthenticationTicket(
                identity, properties));
        }
    }
}
