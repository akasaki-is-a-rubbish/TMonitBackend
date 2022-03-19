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
        // already done in startup(Program.cs).
        protected override void OnConfiguring(DbContextOptionsBuilder options)
                    => options.UseMySql("Server=cloud.akasaki.space;Port=3306;Database=tmonitbackend;Uid=akasaki;Pwd=pwd;connect timeout=100;default command timeout=200;",
                    ServerVersion.AutoDetect("Server=cloud.akasaki.space;Port=3306;Database=tmonitbackend;Uid=akasaki;Pwd=pwd;connect timeout=100;default command timeout=200;"));

        // public DatabaseContext(DbContextOptionsBuilder options):base(options.Options){
        // }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}