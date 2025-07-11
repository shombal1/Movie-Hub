

using FluentValidation;

namespace MovieHub.AI.Narrator.Domain.UseCases.GenerateMovieDescription;

public class GenerateMovieDescriptionCommandValidator : AbstractValidator<GenerateMovieDescriptionCommand>
{
    public GenerateMovieDescriptionCommandValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty().WithErrorCode("Empty");

        RuleFor(x => x.MovieId)
            .NotEmpty().WithErrorCode("Invalid");
    }
}