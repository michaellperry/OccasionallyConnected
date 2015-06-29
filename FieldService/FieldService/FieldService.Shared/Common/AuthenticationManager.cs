using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;
using RoverMob.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;

namespace FieldService.Common
{
    public class AuthenticationManager : Process
    {
        private string _accessToken = result.AccessToken;
        public AuthenticationManager()
        {
        }

        public void Authenticate()
        {
            Perform(async delegate
            {
                await InternalAuthenticateAsync();
            });
        }

        private async Task InternalAuthenticateAsync()
        {
            string authority = "https://login.windows.net/mperryfacetedworlds.onmicrosoft.com";
            string resource = "https://mperryfacetedworlds.onmicrosoft.com/fieldservicedistributor";
            string clientId = "e888dc86-3f7d-40dc-b9ef-296b38b223cc";
            Uri redirectUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri();

#if WINDOWS_PHONE_APP
#else
            var context = new AuthenticationContext(authority);
            var result = await context.AcquireTokenAsync(
                resource, clientId, redirectUri);
#endif

        }
    }
}
