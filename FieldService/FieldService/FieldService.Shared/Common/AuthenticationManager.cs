using Newtonsoft.Json.Linq;
using RoverMob.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.Common
{
    public class AuthenticationManager : Process
    {
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
#if WINDOWS_PHONE_APP
#else
#endif
        }
    }
}
