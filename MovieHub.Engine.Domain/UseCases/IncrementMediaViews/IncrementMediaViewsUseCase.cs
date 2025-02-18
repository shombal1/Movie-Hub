using FluentValidation;
using MediatR;

namespace MovieHub.Engine.Domain.UseCases.IncrementMediaViews;

public class IncrementMediaViewsUseCase(
    IValidator<IncrementMediaViewsCommand> validator,
    IIncrementMediaViewsStorage incrementMediaViewsStorage) : IRequestHandler<IncrementMediaViewsCommand,Unit>
{
    public async Task<Unit> Handle(IncrementMediaViewsCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        await incrementMediaViewsStorage.Increment(request.MediaId,cancellationToken);
        
        return Unit.Value;
    }
}