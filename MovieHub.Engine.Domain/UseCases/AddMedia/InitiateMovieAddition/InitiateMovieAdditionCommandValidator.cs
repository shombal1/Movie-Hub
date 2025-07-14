using FluentValidation;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.InitiateMovieAddition;

public class InitiateMovieAdditionCommandValidator : AbstractValidator<InitiateMovieAdditionCommand>
{
    public InitiateMovieAdditionCommandValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithErrorCode("Empty")
            .MaximumLength(100).WithErrorCode("TooLong");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithErrorCode("TooLong");

        RuleFor(x => x.ReleasedAt)
            .NotEmpty().WithErrorCode("Empty")
            .Must(date => date <= DateOnly.FromDateTime(timeProvider.GetUtcNow().Date))
            .WithErrorCode("InvalidDate");

        RuleFor(x => x.Countries)
            .NotEmpty().WithErrorCode("Empty")
            .Must(countries => countries.All(c => !string.IsNullOrWhiteSpace(c)))
            .WithErrorCode("Invalid");

        RuleFor(x => x.Genres)
            .NotEmpty().WithErrorCode("Empty")
            .Must(genres => genres.All(g => !string.IsNullOrWhiteSpace(g)))
            .WithErrorCode("Invalid");

        RuleFor(x => x.DirectorIds)
            .NotEmpty().WithErrorCode("Empty");

        RuleFor(x => x.ActorIds)
            .NotEmpty().WithErrorCode("Empty");

        RuleFor(x => x.AgeRating)
            .NotEmpty().WithErrorCode("Empty")
            .MaximumLength(10).WithErrorCode("TooLong");

        RuleFor(x => x.Budget)
            .Must(budget => budget is null or > 0)
            .WithErrorCode("Invalid")
            .When(x => x.Budget.HasValue);
    }
}