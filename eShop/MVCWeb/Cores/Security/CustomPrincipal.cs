using System.Linq;
using System.Security.Principal;

namespace MVCWeb.Cores.Security
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role)
        {
            return Roles.Any(role.Contains);
        }
        public bool IsAdmin()
        {
            return IsInRole("Admin");
        }
        public CustomPrincipal(string username)
        {
            Identity = new GenericIdentity(username);
        }

        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public string[] Roles { get; set; }
    }

    public class CustomPrincipalSerializeModel
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public string[] Roles { get; set; }
    }
}