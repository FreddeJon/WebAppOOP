using System.Security.Claims;

namespace WebAppOOP.BusinessLogic.Authentication.UserAuthentication
{
    public class Authentication
    {
        public bool IsAuthenticated { get; private set; }
        public ClaimsPrincipal Principal { get; private set; }
        public string Message { get; private set; }

        public Authentication(bool isAuthenticated, ClaimsPrincipal principal, string message)
        {
            IsAuthenticated = isAuthenticated;
            Principal = principal;
            Message = message;
        }
    }
}
