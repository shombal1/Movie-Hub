using FluentValidation;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.GetMoviePartUploadUrl;

public class GetMoviePartUploadUrlQueryValidator : AbstractValidator<GetMoviePartUploadUrlQuery>
{
    public GetMoviePartUploadUrlQueryValidator()
    {
        RuleFor(x => x.UploadId)
            .NotEmpty().WithErrorCode("Empty");

        RuleFor(x => x.PartNumber)
            .GreaterThan(0).WithErrorCode("Invalid")
            .LessThanOrEqualTo(1000).WithErrorCode("TooLarge");

        RuleFor(x => x.Key)
            .NotEmpty().WithErrorCode("Empty")
            .MaximumLength(1024).WithErrorCode("TooLong");
    }
}