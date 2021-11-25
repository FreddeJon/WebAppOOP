using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WebAppOOP.BusinessLogic.Authentication.UserAuthentication.Interfaces;
using WebAppOOP.BusinessLogic.Hashing;
using WebAppOOP.BusinessLogic.Hashing.Interface;
using WebAppOOP.Core.ModelDTOS;
using WebbAppOOP.Data.DataAccess.Interfaces;

namespace WebAppOOP.BusinessLogic.Authentication.UserAuthentication
{
    public class CookieAuthentication : IAuthentication
    {
        private readonly IDataAccess<User> _userDataAccess;
        private readonly IHash _hash;

        public CookieAuthentication(IDataAccess<User> userDataAccess, IHash hash)
        {
            _userDataAccess = userDataAccess;
            _hash = hash;
        }
        public Authentication AuthenticateUser(UserCredential userCredential)
        {
            var user = _userDataAccess.GetAll().FirstOrDefault(x => x.Email == userCredential.Email);
            if (user is null) return new Authentication(isAuthenticated: false, principal: null, message: "User not Found");


            if (_hash.HashString(password: userCredential.Password, salt: user.Salt) == user.HashedPassword)
            {
                return new Authentication(isAuthenticated: true, principal: GetClaimsPrincipal(user), message: $"Welcome {user.Email}");
            }

            return new Authentication(isAuthenticated: false, principal: null, message: "Invalid Password");
        }

        private bool CheckIfUserExists(UserCredential userCredential)
        {
            var users = _userDataAccess.GetAll();
            var exists = users.Any(x => x.Email == userCredential.Email);
            return exists;
        }

        public Authentication CreateUser(UserCredential userCredential)
        {

            if (CheckIfUserExists(userCredential)) return new Authentication(isAuthenticated: false, principal: null, message: "User already exists");

            var salt = _hash.GenerateSalt();
            var hashedPassword = _hash.HashString(password: userCredential.Password, salt: salt);
            var user = new User { Email = userCredential.Email, Key = hashedPassword, HashedPassword = hashedPassword, Id = _userDataAccess.GetNewID(), Salt = salt, UserType = UserType.USER };
            _userDataAccess.Add(user);

            return new Authentication(isAuthenticated: true, principal: GetClaimsPrincipal(user), message: $"Welcome {user.Email}");
        }

        private static ClaimsPrincipal GetClaimsPrincipal(User user)
        {
            List<Claim> claims;
            if (user.UserType == UserType.ADMIN)
            {
                claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Sid, user.Key),
                    new Claim("Admin", "True"),
                    new Claim("User", "True")
                };
            }
            else
            {
                claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Sid, user.Key),
                    new Claim("User", "True")
                };
            }

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            return claimsPrincipal;
        }
    }
}
