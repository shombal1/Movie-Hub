namespace MovieHub.AI.Narrator.Domain.UseCases.GenerateMovieDescription;

public record GenerateMovieDescriptionCommand(string Key, Guid MovieId);