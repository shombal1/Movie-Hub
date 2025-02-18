using Mapster;
using MovieHub.Engine.Domain.Models;
using MovieHub.Engine.Storage.Entities;

namespace MovieHub.Engine.Storage.Mapping;

public class StorageRegistry : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<MediaEntity, Media>()
            .Map(dest => dest.Id, src => src.Id) 
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.ReleasedAt, src => src.ReleasedAt)
            .Map(dest => dest.PublishedAt, src => src.PublishedAt)
            .Map(dest => dest.Countries, src => src.Countries)
            .Map(dest => dest.Genres, src => src.Genres)
            .Map(dest => dest.Director, src => src.Director)
            .Map(dest=>dest.Views,src =>src.Views)
            .Include<MovieEntity,Movie>()
            .Include<SeriesEntity,Series>();

        config.NewConfig<MovieEntity, Movie>()
            .Map(dest => dest.Quality, src => src.Quality)
            .Inherits<MediaEntity,Media>();
        
        config.NewConfig<SeriesEntity, Series>()
            .Map(dest => dest.CountSeasons, src => src.CountSeasons)
            .Map(dest => dest.CountEpisodes, src => src.CountEpisodes)
            .Inherits<MediaEntity,Media>();
    }
}