using FluentAssertions;
using MovieHub.Engine.Storage.Common;

namespace MovieHub.Engine.Storage.Tests;

public class TextNormalizerShould
{
    [Fact]
    public void NormalizeForFileName_WhenInputIsEmpty_ShouldReturnFallbackValue()
    {
        string input = "";

        string result = FileNameNormalizer.NormalizeForFileName(input);

        result.Should().Be("file");
    }

    [Fact]
    public void NormalizeForFileName_WithEnglishText_ShouldReturnNormalized()
    {
        string input = "The Shawshank Redemption";

        string result = FileNameNormalizer.NormalizeForFileName(input);

        result.Should().Be("the-shawshank-redemption");
    }
    
    [Fact]
    public void NormalizeForFileName_WithSpecialCharacters_ShouldReplaceWithDashes()
    {
        string input = "Movie: The Return! (Part 2)";

        string result = FileNameNormalizer.NormalizeForFileName(input);

        result.Should().Be("movie-the-return-part-2");
    }

    [Fact]
    public void NormalizeForFileName_WithCyrillicText_ShouldTransliterate()
    {
        string input = "Звёздные войны: Эпизод IV";

        string result = FileNameNormalizer.NormalizeForFileName(input);

        result.Should().Be("zvezdnye-voiny-epizod-iv");
    }
    
    [Fact]
    public void NormalizeForFileName_WithCustomFallback_ShouldUseProvidedFallback()
    {
        string input = "";
        string fallback = "custom-name";

        string result = FileNameNormalizer.NormalizeForFileName(input, fallback);

        result.Should().Be(fallback);
    }

    [Fact]
    public void NormalizeForFileName_WithMultipleSpacesAndDashes_ShouldNormalizeToSingleDashes()
    {
        string input = "Movie   Title  --  With__Multiple___Spaces";

        string result = FileNameNormalizer.NormalizeForFileName(input);

        result.Should().Be("movie-title-with-multiple-spaces");
    }

    [Fact]
    public void NormalizeForFileName_WithAccentedCharacters_ShouldRemoveAccents()
    {
        string input = "Café Résumé Naïve";

        string result = FileNameNormalizer.NormalizeForFileName(input);

        result.Should().Be("cafe-resume-naive");
    }

    [Fact]
    public void NormalizeForFileName_ResultingShouldBeEmptyString_ShouldReturnFallback()
    {
        string input = "!@#$%^&*()";

        string result = FileNameNormalizer.NormalizeForFileName(input);

        result.Should().Be("file");
    }
    
    [Fact]
    public void NormalizeForFileName_WithAlreadyNormalizedInput_ShouldReturnSameValue()
    {
        string input = "already-normalized-name";
        
        string result = FileNameNormalizer.NormalizeForFileName(input);
        
        result.Should().Be(input);
    }
}