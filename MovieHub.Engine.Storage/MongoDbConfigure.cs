namespace MovieHub.Engine.Storage;

public class MongoDbConfigure
{
    public string ConnectionString { get; set; }
    public string NameDataBase { get; set; }
    public string NameMediaCollection { get; set; }
    public string NameMovieBasketCollection { get; set; }
    public string NameUserCollection { get; set; }
}