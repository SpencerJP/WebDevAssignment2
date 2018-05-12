using ASPNETAssignment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ASPNETAssignment.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            // In Startup iam creating first Admin Role and creating a default Admin User    
            bool x = await roleManager.RoleExistsAsync("Owner");
            if (x)
            {


                var role = new IdentityRole
                {
                    Name = "Owner"
                };
                await roleManager.CreateAsync(role);            

                var user = new ApplicationUser
                {
                    UserName = "testowner",
                    Email = "testowner@gmail.com"
                };

                string userPWD = "password";

                var chkUser = await userManager.CreateAsync(user, userPWD);
                
                if (chkUser.Succeeded)
                {
                    var result = userManager.AddToRoleAsync(user, "Owner");

                } 


                var role2 = new IdentityRole
                {
                    Name = "Franchisee"
                };
                await roleManager.CreateAsync(role2);

                var user2 = new ApplicationUser
                {
                    UserName = "testfranchisee",
                    Email = "testfranchisee@gmail.com"
                };
                
                var chkUser2 = await userManager.CreateAsync(user2, userPWD);

                if (chkUser2.Succeeded)
                {
                    var result = userManager.AddToRoleAsync(user2, "Franchisee");

                }

                var role3 = new IdentityRole
                {
                    Name = "Customer"
                };
                await roleManager.CreateAsync(role3);

                var user3 = new ApplicationUser
                {
                    UserName = "testcustomer",
                    Email = "testcustomer@gmail.com"
                };
                

                var chkUser3 = await userManager.CreateAsync(user3, userPWD);

                if (chkUser3.Succeeded)
                {
                    var result = userManager.AddToRoleAsync(user3, "Customer");

                }

                var role4 = new IdentityRole
                {
                    Name = "Admin"
                };
                await roleManager.CreateAsync(role4);

                var myUser = await userManager.FindByEmailAsync("spenceroone@hotmail.com");
                var result1 = userManager.AddToRoleAsync(myUser, "Admin");
            }
        }
    }
}
