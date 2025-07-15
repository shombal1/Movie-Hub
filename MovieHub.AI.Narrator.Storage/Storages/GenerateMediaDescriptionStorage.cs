using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MovieHub.AI.Narrator.Domain.UseCases.GenerateMovieDescription;

namespace MovieHub.AI.Narrator.Storage.Storages;

public class GenerateMediaDescriptionStorage(
    IHttpClientFactory httpClientFactory,
    IOptions<GenerateMediaDescriptionOptions> optionsData) 
    : IGenerateMediaDescriptionStorage
{
    private readonly GenerateMediaDescriptionOptions _options = optionsData.Value;
    
    public async Task<string> Generate(string audioText, CancellationToken cancellationToken)
    {
        try
        {
            using var httpClient = httpClientFactory.CreateClient(_options.ClientName);

            var request = CreateRequest(audioText);
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{_options.BaseUrl}/chat/completions")
            {
                Content = content
            };
            httpRequest.Headers.Add("Authorization", $"Bearer {_options.ApiKey}");

            var response = await httpClient.SendAsync(httpRequest, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"API request failed with status {response.StatusCode}");
            }

            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
            return ExtractContentFromResponse(responseJson);
        }
        catch (Exception ex) when (ex is not (ArgumentException or InvalidOperationException))
        {
            throw new InvalidOperationException("Failed to generate media description", ex);
        }
    }

    private object CreateRequest(string audioText)
    {
        return new
        {
            model = _options.Model,
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = $"{_options.SystemPrompt}\nВот диалоги:\n{audioText}"
                }
            }
        };
    }

    private string ExtractContentFromResponse(string responseJson)
    {
        try
        {
            var responseObject = JsonSerializer.Deserialize<JsonElement>(responseJson);
            return responseObject
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "";
        }
        catch (KeyNotFoundException ex)
        {
            throw new InvalidOperationException($"Invalid API response format. Response: {responseJson}", ex);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Failed to parse API response. Response: {responseJson}s", ex);
        }
    }
}