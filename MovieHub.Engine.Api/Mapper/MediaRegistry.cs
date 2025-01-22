using Mapster;
using MovieHub.Engine.Api.Models.Responses;
using MovieHub.Engine.Domain.Models;

namespace MovieHub.Engine.Api.Mapper;

public class MediaRegistry : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Media, MediaDto>()
            .Map(dest => dest.Id, src => src.Id) 
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.ReleasedAt, src => src.ReleasedAt)
            .Map(dest => dest.PublishedAt, src => src.PublishedAt)
            .Map(dest => dest.Country, src => src.Country)
            .Map(dest => dest.Genre, src => src.Genre)
            .Map(dest => dest.Director, src => src.Director)
            .Include<Movie,MovieDto>()
            .Include<Series,SeriesDto>();

        config.NewConfig<Movie, MovieDto>()
            .Map(dest => dest.Quality, src => src.Quality)
            .Inherits<Media,MediaDto>();
        
        config.NewConfig<Series, SeriesDto>()
            .Map(dest => dest.CountSeasons, src => src.CountSeasons)
            .Map(dest => dest.CountEpisodes, src => src.CountEpisodes)
            .Inherits<Media,MediaDto>();
    }
}