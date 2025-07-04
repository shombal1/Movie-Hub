﻿using System.Text.Json.Serialization;

namespace MovieHub.Engine.Api.Models.Responses;

[JsonDerivedType(typeof(MovieDto),"movie")] 
[JsonDerivedType(typeof(SeriesDto),"series")]
public class MediaDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateOnly ReleasedAt { get; set; }
    public DateTimeOffset PublishedAt { get; set; }
    public long Views { get; set; }
    public IEnumerable<string> Countries { get; set; } = null!;
    public IEnumerable<string> Genres { get; set; } = null!;
}