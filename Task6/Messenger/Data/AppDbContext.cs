using Messenger.Models;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _config;
        public DbSet<User> Users {get;set;}
        public DbSet<Message> Messages {get;set;}

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration config)
        : base(options)
        {
            _config = config;
        }
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasIndex(p => p.Name)
                .IsUnique();
                
        }
    }
}