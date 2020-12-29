using ApiWorld.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiWorld.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SuperStar> SuperStars { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Manager> Managers { get; set; }
    }
}
