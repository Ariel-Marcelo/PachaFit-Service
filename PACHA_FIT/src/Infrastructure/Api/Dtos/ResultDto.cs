using System.Text.Json.Serialization;
using PACHA_FIT.Core.Domain.Shared;
using PACHA_FIT.Core.Domain.Shared.ResultPattern;

namespace PACHA_FIT.Infrastructure.Api.Dtos;

public class ResultDto<T> : IResult
{
    [JsonPropertyName("isSuccess")]
    public bool IsSuccess { get; set; }

    [JsonPropertyName("value")]
    public T? Value { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("statusCode")]
    public ErrorType StatusCode { get; set; }

    public object? GetValue() => Value;
}
