using System.ComponentModel;

namespace MovieHub.MediaCompressor;

public enum QualityType
{
    [Description("360p")]
    P360 = 0,
    [Description("480p")]
    P480 = 1,
    [Description("720p")]
    P720 = 2,
    [Description("1080p")]
    P1080 = 3,
    [Description("2k")]
    K2 = 4,
    [Description("4k")]
    K4 = 5
}
