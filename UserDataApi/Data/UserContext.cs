using Microsoft.EntityFrameworkCore;
using UserDataApi.Models;

namespace UserDataApi.Data;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Entry> Entries { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Entry>()
            .HasKey(c => new { c.Id, c.UserId });
    }
}