

using GameWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameWeb.Data
{
    public class AppDbContext : IdentityUserContext<IdentityUser>
    {
        private readonly IConfiguration _config;

        public DbSet<Game>? Games {get;set;}
        public DbSet<Invite>? Invites {get;set;}
        public DbSet<Board>? Boards {get;set;}

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration config)
        : base(options)
    {
        _config = config;
    }
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Game>()
                        .Property(p=>p.Status)
                        .HasConversion<string>();
               
                
        }
    }
}