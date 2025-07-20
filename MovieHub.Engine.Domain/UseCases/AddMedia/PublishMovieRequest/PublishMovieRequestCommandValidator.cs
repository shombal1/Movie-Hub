using FluentValidation;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.PublishMovieRequest;

public class PublishMovieRequestCommandValidator : AbstractValidator<PublishMovieRequestCommand>
{
    public PublishMovieRequestCommandValidator()
    {
        RuleFor(x => x.MovieRequestId)
            .NotEmpty().WithErrorCode("Empty");
    }
}