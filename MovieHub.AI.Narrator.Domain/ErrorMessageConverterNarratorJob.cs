using System.Text.Json;
using Amazon.S3;

namespace MovieHub.AI.Narrator.Domain;

public static class ErrorMessageConverterNarratorJob
{
    public static string ConvertToHumanReadableMessage(Exception ex)
    {
        return ex switch
        {
            HttpRequestException => "Ошибка при обращении к API для генерации описания",
            JsonException => "Ошибка при обработке ответа от API для генерации описания",
            InvalidOperationException invalidEx when invalidEx.Message.Contains("Invalid API response format") => "Неизвестный ответ от API для генерации описания",
            InvalidOperationException invalidEx when invalidEx.Message.Contains("Failed to generate media description") => "Ошибка при генерации описания",
            AmazonS3Exception { StatusCode: System.Net.HttpStatusCode.NotFound } => "Медиафайл не найден в хранилище",
            AmazonS3Exception { StatusCode: System.Net.HttpStatusCode.Forbidden } => "Нет доступа к медиафайлу",
            AmazonS3Exception { StatusCode: System.Net.HttpStatusCode.ServiceUnavailable } => "Сервис хранения временно недоступен",
            _ => "Произошла неизвестная ошибка"

        };
    }
}