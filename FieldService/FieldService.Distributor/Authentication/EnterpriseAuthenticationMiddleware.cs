using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
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