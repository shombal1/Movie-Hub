namespace MovieHub.Engine.Storage;

public class MongoDbConfigure
{

    public string NameDataBase { get; set; }
    public string NameMediaCollection { get; set; }
    public string NameMediaBasketCollection { get; set; }
    public string NameUserCollection { get; set; }
    public string NameSeasonCollection { get; set; }
    public string NameAdditionMediaInfoCollection { get; set; }
    public string NameDomainEventCollection { get; set; }
}