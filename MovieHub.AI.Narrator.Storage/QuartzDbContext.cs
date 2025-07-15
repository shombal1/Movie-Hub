using AppAny.Quartz.EntityFrameworkCore.Migrations;
using AppAny.Quartz.EntityFrameworkCore.Migrations.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using MovieHub.AI.Narrator.Storage.QuartzEntities;

namespace MovieHub.AI.Narrator.Storage;

public class QuartzDbContext(DbContextOptions<QuartzDbContext> options) : DbContext(options)
{
    public DbSet<FailedNarratorJobEntity> FailedNarratorJobs { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.AddQuartz(options => options.UsePostgreSql());
    }
}