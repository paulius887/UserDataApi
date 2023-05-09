using Microsoft.EntityFrameworkCore;
using UserDataApi.Models;

namespace UserDataApi.Data;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Comment>()
            .HasKey(c => new { c.Id, c.UserId });
    }
}