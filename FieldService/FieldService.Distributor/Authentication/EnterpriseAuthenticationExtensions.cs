using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FieldService.Distributor.Authentication
{
    public static class EnterpriseAuthenticationExtensions
    {
        public static IAppBuilder UseEnterpriseAuthentication(
            this IAppBuilder app)
        {
            return app.Use(
                typeof(EnterpriseAuthenticationMiddleware),
                new EnterpriseAuthenticationOptions());
        }
    }
}