using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Ahmed Gawesh",
                    Email = "ahmed.gawesh@gmail.com",
                    UserName = "ahmed.gawesh",
                    PhoneNumber = "01141021702",
                    Address = new Address()
                    {
                        FirstName = "Ahemd",
                        LastName = "Gawesh",
                        Street="123street",
                        City = "Cairo",
                        Country = "Egypt",

                    }
                };
                await userManager.CreateAsync(user, "P@ssw0rd");
            }

        }

    }
}
