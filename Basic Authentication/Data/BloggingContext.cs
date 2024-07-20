using Basic_Authentication.Models;

using Microsoft.EntityFrameworkCore;

namespace Basic_Authentication.Data;

public class BloggingContext(DbContextOptions<BloggingContext> options) : DbContext(options)
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseInMemoryDatabase("BloggingDB1");
    }
}