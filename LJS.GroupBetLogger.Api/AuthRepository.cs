using LJS.GroupBetLogger.Api.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LJS.GroupBetLogger.Api.Extensions;
using LJS.GroupBetLogger.Api.DAL;

namespace LJS.GroupBetLogger.Api
{
    public class AuthRepository : IDisposable
    {
        GroupBetLoggerContext db;
        UserManager<User> userManager;

        public AuthRepository(GroupBetLoggerContext context)
        {
            db = context;
            userManager = new UserManager<User>(new UserStore<User>(context));
        }

        public async Task<IdentityResult> RegisterUser(RegisterModel userModel)
        {
            User user = new User
            {
                UserName = userModel.UserName,
                Email = userModel.Email,
            };

            var result = await userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<User> FindUser(string userNameOrEmail, string password)
        {
            User user = await userManager.FindByNameOrEmailAsync(userNameOrEmail, password);

            return user;
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }
    }
}