namespace MovieHub.AI.Narrator.Domain.UseCases.GenerateMovieDescription;

public interface IGenerateMediaDescriptionUseCase
{
    public Task GenerateMediaDescription(GenerateMovieDescriptionCommand request,
        CancellationToken cancellationToken);
}