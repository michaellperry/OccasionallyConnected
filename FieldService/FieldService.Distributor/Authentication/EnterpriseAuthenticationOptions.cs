using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.Distributor.Authentication
{
    public class EnterpriseAuthenticationOptions : AuthenticationOptions
    {
        public EnterpriseAuthenticationOptions()
            : base("Enterprise")
        {
        }
    }
}
