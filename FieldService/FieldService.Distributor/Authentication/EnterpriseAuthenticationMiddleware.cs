using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;

namespace FieldService.Distributor.Authentication
{
    public class EnterpriseAuthenticationMiddleware :
        AuthenticationMiddleware<EnterpriseAuthenticationOptions>
    {
        public EnterpriseAuthenticationMiddleware(
            OwinMiddleware next,
            EnterpriseAuthenticationOptions options)
            : base(next, options)
        {
        }

        protected override AuthenticationHandler<EnterpriseAuthenticationOptions> CreateHandler()
        {
            return new EnterpriseAuthenticationHandler();
        }
    }
}