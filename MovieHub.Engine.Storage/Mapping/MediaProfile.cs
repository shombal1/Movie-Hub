using AutoMapper;
using MovieHub.Engine.Domain.Models;
using MovieHub.Engine.Storage.Entities;

namespace MovieHub.Engine.Storage.Mapping;

public class MediaProfile : Profile
{
    public MediaProfile()
    {
        CreateMap<MediaEntity, Media>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.ReleasedAt, opt => opt.MapFrom(src => src.ReleasedAt))
            .ForMember(dest => dest.PublishedAt, opt => opt.MapFrom(src => src.PublishedAt))
            .ForMember(dest => dest.Countries, opt => opt.MapFrom(src => src.Countries))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres))
            .ForMember(dest => dest.Directors, opt => opt.MapFrom(src => src.Directors))
            .ForMember(dest => dest.Views, opt => opt.MapFrom(src => src.Views))
            .Include<MovieEntity, Movie>()
            .Include<SeriesEntity, Series>();
        
        CreateMap<MovieEntity, Movie>()
            .ForMember(dest => dest.Quality, opt => opt.MapFrom(src => src.Quality))
            .IncludeBase<MediaEntity, Media>();
        
        CreateMap<SeriesEntity, Series>()
            .ForMember(dest => dest.CountSeasons, opt => opt.MapFrom(src => src.CountSeasons))
            .ForMember(dest => dest.CountEpisodes, opt => opt.MapFrom(src => src.CountEpisodes))
            .IncludeBase<MediaEntity, Media>();

        CreateMap<MovieRequestEntity, MovieRequest>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

        CreateMap<Models.ProcessingStatus,ProcessingStatus>();
    }
}