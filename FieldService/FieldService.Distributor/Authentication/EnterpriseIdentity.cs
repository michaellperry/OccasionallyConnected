using System.Security.Principal;

namespace FieldService.Distributor.Authentication
{
    public class EnterpriseIdentity : IIdentity
    {
        public string Name { get; set; }

        public string AuthenticationType
        {
            get { return "Enterprise"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }
    }
}
