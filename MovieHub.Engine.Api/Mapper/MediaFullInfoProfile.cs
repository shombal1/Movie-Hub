using AutoMapper;
using MovieHub.Engine.Api.Models.Responses;
using MovieHub.Engine.Domain.Models;

namespace MovieHub.Engine.Api.Mapper;

public class MediaFullInfoProfile : Profile
{
    public MediaFullInfoProfile()
    {
        CreateMap<MediaFullInfo, MediaFullInfoDto>()
            .Include<MovieFullInfo, MovieFullInfoDto>()
            .Include<SeriesFullInfo, SeriesFullInfoDto>();

        CreateMap<MovieFullInfo, MovieFullInfoDto>()
            .IncludeBase<MediaFullInfo, MediaFullInfoDto>();

        CreateMap<SeriesFullInfo, SeriesFullInfoDto>()
            .IncludeBase<MediaFullInfo, MediaFullInfoDto>();

        CreateMap<Season, SeasonDto>();
        CreateMap<Episode, EpisodeDto>();
    }
}