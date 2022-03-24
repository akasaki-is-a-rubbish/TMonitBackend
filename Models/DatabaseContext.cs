using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TMonitBackend.Models;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace TMonitBackend.Models
{
    public class DatabaseContext : IdentityDbContext<User, IdentityRole<long>, long>
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<UserBehavior> UserBehaviors { get; set; }
        public DbSet<CommonImage> Images {get; set;}

        public DatabaseContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}