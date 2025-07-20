using AutoMapper;
using MovieHub.Engine.Api.Models.Responses;
using MovieHub.Engine.Domain.Models;

namespace MovieHub.Engine.Api.Mapper;

public class MediaProfile : Profile
{
    public MediaProfile()
    {
        CreateMap<Media, MediaDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.ReleasedAt, opt => opt.MapFrom(src => src.ReleasedAt))
            .ForMember(dest => dest.PublishedAt, opt => opt.MapFrom(src => src.PublishedAt))
            .ForMember(dest => dest.Countries, opt => opt.MapFrom(src => src.Countries))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres))
            .ForMember(dest => dest.Views, opt => opt.MapFrom(src => src.Views))
            .Include<Movie, MovieDto>()
            .Include<Series, SeriesDto>();

        CreateMap<Movie, MovieDto>()
            .IncludeBase<Media, MediaDto>();

        CreateMap<Series, SeriesDto>()
            .ForMember(dest => dest.CountSeasons, opt => opt.MapFrom(src => src.CountSeasons))
            .ForMember(dest => dest.CountEpisodes, opt => opt.MapFrom(src => src.CountEpisodes))
            .IncludeBase<Media, MediaDto>();
    }
}