using ASPNETAssignment.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAssignment.Models
{
    public class SeedUsersRoles
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for roles
                if (context.Roles.Any()) {
                    return;
                }

                context.Roles.AddRange(
                    new IdentityRole
                    {
                        Name = "Owner"
                    },

                    new IdentityRole
                    {
                        Name = "FranchiseHolder"
                    },

                    new IdentityRole
                    {
                        Name = "Customer"
                    }
                );
                var user1 = new ApplicationUser
                {
                    UserName = "testowner",
                    Email = "testowner@gmail.com",

                };
                var user2 = new ApplicationUser
                {

                    UserName = "testfranchise",
                    Email = "testfranchise@gmail.com",
                };
                var user3 = new ApplicationUser
                {
                    UserName = "testcustomer",
                    Email = "testcustomer@gmail.com",
                };

                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user1, "abc123");
                user1.PasswordHash = hashed;
                user2.PasswordHash = hashed;
                user3.PasswordHash = hashed;

                var userStore = new UserStore<ApplicationUser>(context);
                var result = userStore.CreateAsync(user1);
                var result2 = userStore.CreateAsync(user2);
                var result3 = userStore.CreateAsync(user3);
                context.SaveChanges();
            }
        }
    }
}
