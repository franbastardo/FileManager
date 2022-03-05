using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using FileManager.Models;

namespace FileManager.Data
{
    public class FileManagerContext : DbContext
    {
        public FileManagerContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Credentials> Credentials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Credentials)
                .WithOne(c => c.User)
                .HasForeignKey<Credentials>(c => c.UserId);
        }
    }
}
