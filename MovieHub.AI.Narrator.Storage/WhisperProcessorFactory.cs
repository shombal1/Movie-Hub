using Whisper.net;

namespace MovieHub.AI.Narrator.Storage;

public class WhisperProcessorFactory(WhisperFactoryProvider factoryProvider)
{
    public async Task<WhisperProcessor> Create()
    {
        var factory = await factoryProvider.GetFactoryAsync();
        return factory.CreateBuilder()
            .WithLanguage("auto")
            .Build();
    }
}