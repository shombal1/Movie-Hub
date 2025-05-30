using FluentValidation;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.StartMovieUpload;

public class StartMovieUploadCommandValidator : AbstractValidator<StartMovieUploadCommand>
{
    public StartMovieUploadCommandValidator()
    {
        RuleFor(x => x.RequestId)
            .NotEmpty().WithErrorCode("Empty");
            
        RuleFor(x => x.ContentType)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode("Empty")
            .Must(BeValidContentType).WithErrorCode("Invalid");
            
        RuleFor(x => x.FileName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode("Empty")
            .Must(BeValidFileName).WithErrorCode("Invalid");
    }
    
    private bool BeValidContentType(string contentType)
    {
        return contentType.StartsWith("video/") || contentType == "application/octet-stream";
    }
    
    private bool BeValidFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName)) return false;

        string[] allowedExtensions = [".mp4", ".avi", ".mkv", ".mov"];
        return allowedExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
    }
}