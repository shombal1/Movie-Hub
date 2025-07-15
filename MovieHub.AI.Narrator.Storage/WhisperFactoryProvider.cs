using System.Collections.Frozen;
using System.Collections.Immutable;
using Whisper.net;
using Whisper.net.Ggml;

namespace MovieHub.AI.Narrator.Storage;

public class WhisperFactoryProvider
{
    public GgmlType ModelType { get; }
    private readonly Lazy<Task<WhisperFactory>> _factoryLazy;

    public static FrozenDictionary<GgmlType, string> ModelNames { get; } = new Dictionary<GgmlType, string>
    {
        { GgmlType.Tiny, "tiny" },
        { GgmlType.Base, "base" },
        { GgmlType.Small, "small" },
        { GgmlType.Medium, "medium" },
        { GgmlType.LargeV1, "largeV1" },
        { GgmlType.LargeV2, "largeV2" },
        { GgmlType.LargeV3, "largeV3" },
        { GgmlType.LargeV3Turbo, "largeV3eTurbo" },
    }.ToFrozenDictionary();
    
    public WhisperFactoryProvider(GgmlType modelType)
    {
        ModelType = modelType;
        _factoryLazy = new Lazy<Task<WhisperFactory>>(Initialize, LazyThreadSafetyMode.ExecutionAndPublication);
    }

    private async Task<WhisperFactory> Initialize()
    {
        string modelName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"ggl-{ModelNames[ModelType]}.bin");
        
        if (File.Exists(modelName)) 
            return WhisperFactory.FromPath(modelName);
        
        await using var modelStream = await WhisperGgmlDownloader.Default.GetGgmlModelAsync(ModelType);
        await using var fileWriter = File.OpenWrite(modelName);
        await modelStream.CopyToAsync(fileWriter);

        return WhisperFactory.FromPath(modelName);
    }

    public Task<WhisperFactory> GetFactoryAsync() => _factoryLazy.Value;
}