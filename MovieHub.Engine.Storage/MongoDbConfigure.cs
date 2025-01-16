namespace MovieHub.Engine.Storage;

public class MongoDbConfigure
{
    public string ConnectionString { get; set; }
    public string NameDataBase { get; set; }
    public string NameMovieCollection { get; set; }
    public string NameMovieBasketCollection { get; set; }
    public string NameUserCollection { get; set; }
}