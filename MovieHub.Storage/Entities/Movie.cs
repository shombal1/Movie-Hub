using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MovieHub.Storage.Entities;

public class Movie
{
    public Guid Id { get; set; }
    [MaxLength(100)] public string Title { get; set; } = "";
    [MaxLength(1000)] public string Description { get; set; } = "";
    public DateOnly ReleasedAt { get; set; }

    public ICollection<MovieBasket> MovieBaskets { get; set; } = null!;
}