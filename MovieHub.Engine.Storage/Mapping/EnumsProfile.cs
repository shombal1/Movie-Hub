using AutoMapper;

namespace MovieHub.Engine.Storage.Mapping;

public class EnumsProfile : Profile
{
    public EnumsProfile()
    {
        CreateMap<Storage.Common.QualityType, Domain.Enums.QualityType>()
            .ConvertUsing((src, destination) => src switch
            {
                Storage.Common.QualityType.P360 => Domain.Enums.QualityType.P360,
                Storage.Common.QualityType.P480 => Domain.Enums.QualityType.P480,
                Storage.Common.QualityType.P720 => Domain.Enums.QualityType.P720,
                Storage.Common.QualityType.P1080 => Domain.Enums.QualityType.P1080,
                Storage.Common.QualityType.K2 => Domain.Enums.QualityType.K2,
                Storage.Common.QualityType.K4 => Domain.Enums.QualityType.K4,
                _ => throw new ArgumentOutOfRangeException(nameof(src), src, "Unexpected quality type")
            });
        
        CreateMap<Storage.Common.ProfessionType, Domain.Enums.ProfessionType>()
            .ConvertUsing((src, destination) => src switch
            {
                Storage.Common.ProfessionType.Actor => Domain.Enums.ProfessionType.Actor,
                Storage.Common.ProfessionType.Director => Domain.Enums.ProfessionType.Director,
                Storage.Common.ProfessionType.Producer => Domain.Enums.ProfessionType.Producer,
                _ => throw new ArgumentOutOfRangeException(nameof(src), src, "Unexpected quality type")
            });
    }
}