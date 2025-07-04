using System.ComponentModel;

namespace MovieHub.Engine.Storage.Common;

public enum ProfessionType
{
    [Description("actor")]    
    Actor = 0,
    [Description("director")]
    Director = 1,
    [Description("producer")]
    Producer = 2
}