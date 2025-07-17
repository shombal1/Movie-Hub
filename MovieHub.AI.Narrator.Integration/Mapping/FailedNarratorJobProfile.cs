using AutoMapper;
using MovieHub.AI.Narrator.Domain.Models;
using MovieHub.AI.Narrator.Integration.Models.Responses;

namespace MovieHub.AI.Narrator.Integration.Mapping;

public class FailedNarratorJobProfile : Profile
{
    public FailedNarratorJobProfile()
    {
        CreateMap<FailedNarratorJob, FailedNarratorJobDto>();
    }
}