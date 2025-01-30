using FluentValidation;

namespace MovieHub.Engine.Domain.UseCases.GetMedia;

public class GetMediaQueryValidator : AbstractValidator<GetMediaQuery>
{
    public GetMediaQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithErrorCode("Invalid");

        RuleFor(x => x.TypeSorting)
            .IsInEnum().WithErrorCode("Invalid");

        RuleFor(x => x.ParameterSorting)
            .IsInEnum().WithErrorCode("Invalid");

        RuleFor(x => x.Countries)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithErrorCode("Empty")
            .ForEach(x => x.NotEmpty()).WithErrorCode("Empty elements");

        RuleFor(x => x.Genres)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithErrorCode("Empty")
            .ForEach(x => x.NotEmpty()).WithErrorCode("Empty elements");

        RuleFor(x => x.Years)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithErrorCode("Empty")
            .ForEach(x => x.ExclusiveBetween(1895, 2100)).WithErrorCode("Empty elements");
    }
}