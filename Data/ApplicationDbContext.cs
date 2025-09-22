using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Maksiupedia.Models;

namespace Maksiupedia.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserInvitation> UserInvitations { get; set; } = null!;
        public DbSet<Photo> Photos { get; set; } = null!;
    }
}
