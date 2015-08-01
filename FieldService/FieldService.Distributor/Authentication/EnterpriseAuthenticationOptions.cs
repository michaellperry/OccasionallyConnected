using Microsoft.Owin.Security;

namespace FieldService.Distributor.Authentication
{
    public class EnterpriseAuthenticationOptions : AuthenticationOptions
    {
        public EnterpriseAuthenticationOptions()
            : base(EnterpriseAuthenticationDefaults.AuthenticationType)
        {
        }
    }
}
