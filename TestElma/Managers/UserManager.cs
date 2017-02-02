using Microsoft.AspNet.Identity;
using System;
using TestElma.Models;

namespace TestElma.Managers
{
    public class UserManager : UserManager<User, Guid>
    {
        public UserManager(IUserStore<User, Guid> store)
            : base(store)
        {
            UserValidator = new UserValidator<User, Guid>(this);
            PasswordValidator = new PasswordValidator();
        }
    }
}