using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Readdit.Models;

namespace Readdit.Areas.Identity.Data
{

    public class ReadditContext : IdentityDbContext<ApplicationUser>
    {
        public ReadditContext(DbContextOptions<ReadditContext> options)
            : base(options)
        {
        }

        public DbSet<Books> Book { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }

}