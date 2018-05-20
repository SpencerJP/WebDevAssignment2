using ASPNETAssignment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAssignment.Data
{
    public static class SeedData
    {

        public static async Task InitializeProducts(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>())
                )

            {

                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                if (context.Transaction.Any())
                {
                    return;
                }
                else
                {
                    await context.SaveChangesAsync();
                }
            }
        }

        public static async Task InitializeRolesAndUsers(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var roles = new[] { "Customer", "Owner", "FranchiseHolder" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role });
                }
            }

            var users = new[] { "testcustomer", "testowner", "testfranchisee" };
            //string password = "MyP@ssword1234!";
            /*
            foreach (var user in users)
            {
                var checkUser = userManager.FindByNameAsync(user);
                try
                {
                    System.Diagnostics.Debug.WriteLine(checkUser.Result.UserName);  // for some reason checkUser == null is not working so this throws if checkUser is null

                }
                catch(Exception)
                {
                    System.Diagnostics.Debug.WriteLine("test3");
                    var newUser = new ApplicationUser()
                    {
                        UserName = user,
                        Email = user + "@gmail.com",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newUser, password);
                }
            } */
            

            await EnsureUserHasRole(userManager, "testcustomer@gmail.com", "Customer");
            await EnsureUserHasRole(userManager, "testowner@gmail.com", "Owner");
            await EnsureUserHasRole(userManager, "testfranchisee@gmail.com", "FranchiseHolder");
        }

        private static async Task EnsureUserHasRole(
            UserManager<ApplicationUser> userManager, string userName, string role)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user != null && !await userManager.IsInRoleAsync(user, role))
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
