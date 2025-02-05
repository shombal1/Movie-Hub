using FluentAssertions;
using MovieHub.Engine.Domain.UseCases.AddMediaToBasket;

namespace MovieHub.Engine.Domain.Tests.AddMediaToBasket;

public class AddMediaToBasketCommandValidatorShould
{
    private readonly AddMediaToBasketCommandValidator _sut = new AddMediaToBasketCommandValidator();

    [Fact]
    public async Task ReturnFailure_WhenRequestIsValid()
    {
        var invalidCommand = new AddMediaToBasketCommand(Guid.Empty);

        var actual = await _sut.ValidateAsync(invalidCommand);

        actual.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task ReturnFailure_WhenRequestIsInvalid()
    {
        var validCommand = new AddMediaToBasketCommand(Guid.Parse("FBD333A6-EC2A-46CB-B030-D7799A4E9AA5"));

        var actual = await _sut.ValidateAsync(validCommand);

        actual.IsValid.Should().BeTrue();
    }
}