using AutoMapper;
using MovieHub.Engine.Domain.Models;
using MovieHub.Engine.Storage.Entities;

namespace MovieHub.Engine.Storage.Mapping;

public class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<PersonEntity, Person>();

        CreateMap<PersonEntity, BasePersonInfo>();

        CreateMap<Models.BasePersonInfo, BasePersonInfo>();

        CreateMap<PersonEntity, Models.BasePersonInfo>();
    }
}