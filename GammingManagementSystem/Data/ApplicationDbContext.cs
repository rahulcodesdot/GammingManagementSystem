using GammingManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GammingManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Campaigns> Campaigns { get; set; }
        public DbSet<ReferralUsers> ReferralUsers { get; set; }
        public DbSet<Games> Games { get; set; }
        public DbSet<Player> Players { get; set; }
    }
}