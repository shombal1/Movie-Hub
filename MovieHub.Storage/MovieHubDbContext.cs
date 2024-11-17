using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MovieHub.Storage.Entities;

namespace MovieHub.Storage;

public class MovieHubDbContext(DbContextOptions<MovieHubDbContext> options) : DbContext(options)
{
    public DbSet<MovieBasket> MovieBaskets { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(MovieHubDbContext))!);

        base.OnModelCreating(modelBuilder);
    }
}