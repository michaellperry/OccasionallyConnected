using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;
using RoverMob.Messaging;
using RoverMob.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;

namespace FieldService.Common
{
    public class AuthenticationManager : Process, IAccessTokenProvider
    {
        private string _accessToken;
        private ImmutableList<TaskCompletionSource<string>> _accessTokenCompletions =
            ImmutableList<TaskCompletionSource<string>>.Empty;

        public AuthenticationManager()
        {
        }

        public Task<string> GetAccessTokenAsync()
        {
            if (!String.IsNullOrEmpty(_accessToken))
                return Task.FromResult(_accessToken);

            var completion = new TaskCompletionSource<string>();
            lock (this)
            {
                _accessTokenCompletions = _accessTokenCompletions.Add(
                    completion);
            }
            Authenticate();

            return completion.Task;
        }

        public void RefreshAccessToken()
        {
            _accessToken = null;
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
            if (!String.IsNullOrEmpty(_accessToken))
            {
                ReceiveAccessToken(_accessToken);
                return;
            }

            string authority = "https://login.windows.net/mperryfacetedworlds.onmicrosoft.com";
            string resource = "https://mperryfacetedworlds.onmicrosoft.com/fieldservicedistributor";
            string clientId = "e888dc86-3f7d-40dc-b9ef-296b38b223cc";
            Uri redirectUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri();

#if WINDOWS_PHONE_APP
            var context = await AuthenticationContext.CreateAsync(
                authority);
            var result = await context.AcquireTokenSilentAsync(
                resource, clientId);

            if (result != null)
                ReceiveAccessToken(result.AccessToken);
            else
                context.AcquireTokenAndContinue(
                    resource, clientId, redirectUri,
                    r => ReceiveAccessToken(r.AccessToken));
#else
            var context = new AuthenticationContext(authority);
            var result = await context.AcquireTokenAsync(
                resource, clientId, redirectUri);

            ReceiveAccessToken(result.AccessToken);
#endif
        }

        private void ReceiveAccessToken(string accessToken)
        {
            _accessToken = accessToken;
            ImmutableList<TaskCompletionSource<string>> completions;
            lock (this)
            {
                completions = _accessTokenCompletions;
                _accessTokenCompletions = ImmutableList<
                    TaskCompletionSource<string>>.Empty;
            }
            foreach (var completion in completions)
                completion.SetResult(_accessToken);
        }
    }
}
