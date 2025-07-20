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
            .ForMember(dest => dest.Views, opt => opt.MapFrom(src => src.Views))
            .Include<MovieEntity, Movie>()
            .Include<SeriesEntity, Series>();

        CreateMap<MovieEntity, Movie>()
            .IncludeBase<MediaEntity, Media>();

        CreateMap<SeriesEntity, Series>()
            .ForMember(dest => dest.CountSeasons, opt => opt.MapFrom(src => src.CountSeasons))
            .ForMember(dest => dest.CountEpisodes, opt => opt.MapFrom(src => src.CountEpisodes))
            .IncludeBase<MediaEntity, Media>();

        CreateMap<MovieRequestEntity, MovieRequest>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

        CreateMap<Models.ProcessingStatus, ProcessingStatus>();


        CreateMap<AdditionMediaInfoEntity, MediaFullInfo>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MediaId))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.MainInfo.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.MainInfo.Description))
            .ForMember(dest => dest.ReleasedAt, opt => opt.MapFrom(src => src.MainInfo.ReleasedAt))
            .ForMember(dest => dest.PublishedAt, opt => opt.MapFrom(src => src.MainInfo.PublishedAt))
            .ForMember(dest => dest.Views, opt => opt.MapFrom(src => src.MainInfo.Views))
            .ForMember(dest => dest.Countries, opt => opt.MapFrom(src => src.MainInfo.Countries))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.MainInfo.Genres))
            .ForMember(dest => dest.Directors, opt => opt.MapFrom(src => src.Directors))
            .ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.Actors))
            .ForMember(dest => dest.AgeRating, opt => opt.MapFrom(src => src.AgeRating))
            .ForMember(dest => dest.Budget, opt => opt.MapFrom(src => src.Budget))
            .Include<AdditionMovieInfoEntity, MovieFullInfo>()
            .Include<AdditionSeriesInfoEntity, SeriesFullInfo>();

        CreateMap<AdditionMovieInfoEntity, MovieFullInfo>()
            .ForMember(dest => dest.AvailableQualities, opt => opt.MapFrom(src => src.AvailableQualities))
            .ForMember(dest => dest.AvailableQualities, opt => opt.MapFrom(src => src.AvailableQualities))
            .IncludeBase<AdditionMediaInfoEntity, MediaFullInfo>();

        CreateMap<AdditionSeriesInfoEntity, SeriesFullInfo>()
            .ForMember(dest => dest.CountSeasons, opt => opt.MapFrom(src => ((SeriesEntity)src.MainInfo).CountSeasons))
            .ForMember(dest => dest.CountEpisodes,
                opt => opt.MapFrom(src => ((SeriesEntity)src.MainInfo).CountEpisodes))
            .ForMember(dest => dest.Seasons, opt => opt.MapFrom(src => src.Seasons))
            .IncludeBase<AdditionMediaInfoEntity, MediaFullInfo>();

        CreateMap<SeasonEntity, Season>();
        CreateMap<Storage.Models.Episode, Episode>()
            .ForMember(dest => dest.EpisodeNumber, opt => opt.MapFrom(src => src.Number))
            .ForMember(dest => dest.StreamingUrl, opt => opt.MapFrom(src => src.StreamingUrl))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title));
    }
}