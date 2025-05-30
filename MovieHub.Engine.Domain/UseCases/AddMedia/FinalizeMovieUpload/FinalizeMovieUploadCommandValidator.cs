using FluentValidation;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.FinalizeMovieUpload;

public class FinalizeMovieUploadCommandValidator : AbstractValidator<FinalizeMovieUploadCommand>
{
    public FinalizeMovieUploadCommandValidator()
    {
        RuleFor(x => x.UploadId)
            .NotEmpty().WithErrorCode("Empty");

        RuleForEach(x => x.Parts)
            .NotNull().WithErrorCode("Empty")
            .ChildRules(part =>
            {
                part.RuleFor(p => p.PartNumber).GreaterThan(0).WithErrorCode("Invalid");
                part.RuleFor(p => p.PartName).NotEmpty().WithErrorCode("Empty");
            });

        RuleFor(x => x.Key)
            .NotEmpty().WithErrorCode("Empty")
            .MaximumLength(1024).WithErrorCode("TooLong");
    }
}