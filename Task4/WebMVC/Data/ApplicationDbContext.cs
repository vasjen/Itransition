using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebMVC.Models;

namespace WebMVC.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly IConfiguration _config;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration config)
        : base(options)
    {
        _config = config;
    }

     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }

    protected override void OnModelCreating (ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser> (b => 
        {
             b.HasIndex(u => u.NormalizedUserName).HasName("UserNameIndex");
             b.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex").IsUnique();
        });
    }
}
