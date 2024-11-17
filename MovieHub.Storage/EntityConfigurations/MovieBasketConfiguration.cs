using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieHub.Storage.Entities;

namespace MovieHub.Storage.EntityConfigurations;

public class MovieBasketConfiguration: IEntityTypeConfiguration<MovieBasket>
{
    public void Configure(EntityTypeBuilder<MovieBasket> builder)
    {
        builder.HasKey(m => m.Id);

        builder.HasOne(m => m.Movie)
            .WithMany(m => m.MovieBaskets)
            .HasForeignKey(m => m.MovieId);
        
        builder.HasOne(m => m.User)
            .WithMany(u => u.MovieBaskets)
            .HasForeignKey(m => m.UserId);

        builder.HasIndex(m => new { m.UserId, m.MovieId })
            .IsUnique()
            .HasDatabaseName("idx_user_and_movie");
    }
}