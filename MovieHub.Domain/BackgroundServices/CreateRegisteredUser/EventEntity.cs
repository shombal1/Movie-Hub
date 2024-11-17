using System.Data.Common;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MovieHub.Domain.BackgroundServices.CreateRegisteredUser;

public class EventEntity
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("clientId")]
    public string? ClientId { get; set; }

    [JsonPropertyName("details")]
    public JsonElement DetailsJson { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("ipAddress")]
    public string? IpAddress { get; set; }

    [JsonPropertyName("realmId")]
    public Guid RealmId { get; set; }
    
    [JsonPropertyName("realmName")]
    public string? RealmName { get; set; }

    [JsonPropertyName("sessionId")]
    public string? SessionId { get; set; }

    [JsonPropertyName("time")]
    public long EventTime { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("userId")]
    public Guid? UserId { get; set;}
}