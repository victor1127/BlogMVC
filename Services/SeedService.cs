using BlogMVC.Data;
using Microsoft.AspNetCore.Identity;
using BlogMVC.Enums;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.Services
{
    public class SeedService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BlogUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedService(ApplicationDbContext context,
                           UserManager<BlogUser> userManager,
                           RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public async Task ManageDataAsyn()
        {
            await SeedRolesAsync();
            await SeedUsersAsync();
        }

        private async Task SeedRolesAsync()
        {
            foreach(var role in Enum.GetNames(typeof(Roles)))
            {
                if(!_context.Roles.Any(r => r.Name == role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private async Task SeedUsersAsync()
        {
            if (_context.Users.Any()) return;

            BlogUser user1 = new BlogUser()
            {
                FirstName = "Victor",
                LastName = "Sosa",
                DateOfBirth = new DateTime(1995, 11, 27),
                Gender = Gender.Male,
                Email = "vsosa01127@gmail.com",
                UserName = "vsosa01127@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "(809) 555-555-5555"
            };

            await _userManager.CreateAsync(user1, "Vi123456789*");
            await _userManager.AddToRoleAsync(user1, Roles.Adm.ToString());

            BlogUser user2 = new BlogUser()
            {
                FirstName = "Alex",
                LastName = "Del Mar",
                DateOfBirth = new DateTime(1993, 11, 27),
                Gender = Gender.Male,
                Email = "alex@gmail.com",
                UserName = "alex@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "(809) 555-4444"
            };

            await _userManager.CreateAsync(user2, "Vi123456789*");
            await _userManager.AddToRoleAsync(user2, Roles.Moderator.ToString());


        }
    }
}
