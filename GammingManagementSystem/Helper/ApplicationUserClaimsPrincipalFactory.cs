using GammingManagementSystem.Data;
using GammingManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace GammingManagementSystem.Helper
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        private readonly ApplicationDbContext _context;
        public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options, ApplicationDbContext context)
            : base(userManager, roleManager, options)
        {
            _context = context;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            var userRole = GetRole(user.Id);
            identity.AddClaim(new Claim("UserFirstName", user.FirstName ?? ""));
            identity.AddClaim(new Claim("UserLastName", user.LastName ?? ""));
            identity.AddClaim(new Claim("UserRole", userRole ?? ""));
            identity.AddClaim(new Claim("UserId", user.Id ?? ""));

            return identity;
        }

        private string GetRole(string userId)
        {
            var response = (from user in _context.ApplicationUser
                            join uRole in _context.UserRoles on user.Id equals uRole.UserId
                            join role in _context.Roles on uRole.RoleId equals role.Id
                            where (user.Id == userId)
                            select new
                            {
                                role.Name
                            }).FirstOrDefault();
            return response.Name;
        }
    }
}
