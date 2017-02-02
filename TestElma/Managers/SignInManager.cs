using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using TestElma.Models;

namespace TestElma.Managers
{
    public class SignInManager : SignInManager<User, Guid>
    {
        public SignInManager(UserManager<User, Guid> userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        { }

        public void SignOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}