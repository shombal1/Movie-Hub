namespace MovieHub.Storage.Entities;

public class User
{
    public Guid Id { get; set; }
    
    public ICollection<MovieBasket> MovieBaskets { get; set; } = null!;
}