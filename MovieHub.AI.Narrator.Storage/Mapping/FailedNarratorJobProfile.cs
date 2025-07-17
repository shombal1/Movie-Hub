using AutoMapper;
using MovieHub.AI.Narrator.Domain.Models;
using MovieHub.AI.Narrator.Storage.QuartzEntities;

namespace MovieHub.AI.Narrator.Storage.Mapping;

public class FailedNarratorJobProfile : Profile
{
    public FailedNarratorJobProfile()
    {
        CreateMap<FailedNarratorJobEntity, FailedNarratorJob>();
    }
}