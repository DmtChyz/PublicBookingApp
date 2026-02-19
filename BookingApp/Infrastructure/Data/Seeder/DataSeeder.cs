    using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data.Seeder
{
    public class DataSeeder
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeeder(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task SeedAsync()
        {
            await AddRolesToApplicationAsync();
            await AddAdminUserAsync();
        }

        private async Task AddRolesToApplicationAsync()
        {
            var roleNames = new List<string> { "admin", "user" };
            foreach (var roleName in roleNames)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
        private async Task AddAdminUserAsync()
        {
            var adminList = new List<IdentityUser>();
            adminList.Add(new IdentityUser
            {
                UserName = "admin123",
                Email = "admin@gmail.com",
                EmailConfirmed = true
            });
            foreach (var admin in adminList)
            {
                if (await _userManager.FindByEmailAsync(admin.Email) == null)
                {
                    var adminUser = new IdentityUser
                    {
                        UserName = admin.UserName,
                        Email = admin.Email,
                        EmailConfirmed = admin.EmailConfirmed
                    };

                    var result = await _userManager.CreateAsync(adminUser, "123qwezxcW@");

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(adminUser, "admin");
                    }
                }
            }
        }
    }
}