using FieldService.Distributor.Authentication;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http;

namespace FieldService.Distributor
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token and enterprise authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(
                OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Distributor",
                routeTemplate: "api/distributor/{topic}",
                defaults: new { controller = "Distributor" }
            );

            config.Routes.MapHttpRoute(
                name: "TechnicianIdentifier",
                routeTemplate: "api/technicianIdentifier/{userId}",
                defaults: new { controller = "TechnicianIdentifier", userId = RouteParameter.Optional }
            );
        }
    }
}
