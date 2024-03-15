using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Readdit.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

namespace Readdit.Areas.Identity.Data
{

    public class ReadditContext : IdentityDbContext<ApplicationUser>
    {
        public ReadditContext(DbContextOptions<ReadditContext> options)
            : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<UserBook> UserBooks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define primary key for IdentityUserLogin
            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey });

            // Your existing configurations
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserBooks)
                .WithOne(ub => ub.User)
                .HasForeignKey(ub => ub.UserId);

            modelBuilder.Entity<UserBook>()
                .HasKey(ub => new { ub.UserId, ub.BookId });

            modelBuilder.Entity<Book>()
                .HasMany(b => b.UserBooks)
                .WithOne(ub => ub.Book)
                .HasForeignKey(ub => ub.BookId);
        }

    }

}