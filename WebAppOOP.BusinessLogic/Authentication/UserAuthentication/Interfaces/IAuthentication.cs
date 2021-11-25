using WebAppOOP.Core.ModelDTOS;

namespace WebAppOOP.BusinessLogic.Authentication.UserAuthentication.Interfaces
{
    public interface IAuthentication
    {
        Authentication CreateUser(UserCredential userCredential);
        Authentication AuthenticateUser(UserCredential userCredential);
    }
}
