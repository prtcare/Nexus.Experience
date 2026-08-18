using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Nexus.Intelligence.Contracts;

namespace Nexus.Products.Chat.Infrastructure.Intelligence;

// The Intelligence API serialises enums as strings. Without JsonStringEnumConverter every
// request fails model binding and returns an empty 400 - this is a known, already-diagnosed
// failure mode, do not rediscover it.
public sealed class HttpIntelligenceClient : IIntelligenceClient
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly HttpClient _httpClient;

    public HttpIntelligenceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IntelligenceTurnResponse> SendTurnAsync(
        IntelligenceTurnRequest request,
        CancellationToken ct = default)
    {
        using var httpResponse = await _httpClient.PostAsJsonAsync(
            "intelligence/v1/turns",
            request,
            SerializerOptions,
            ct);

        await EnsureSuccessAsync(httpResponse, ct);

        var response = await httpResponse.Content.ReadFromJsonAsync<IntelligenceTurnResponse>(
            SerializerOptions,
            ct);

        return response
            ?? throw new IntelligenceClientException(httpResponse.StatusCode, "Empty response body.");
    }

    public async Task ReportResultAsync(
        ResultReport report,
        CancellationToken ct = default)
    {
        using var httpResponse = await _httpClient.PostAsJsonAsync(
            "intelligence/v1/results",
            report,
            SerializerOptions,
            ct);

        await EnsureSuccessAsync(httpResponse, ct);
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage httpResponse, CancellationToken ct)
    {
        if (httpResponse.IsSuccessStatusCode)
        {
            return;
        }

        var body = await httpResponse.Content.ReadAsStringAsync(ct);

        throw new IntelligenceClientException(httpResponse.StatusCode, body);
    }
}

public sealed class IntelligenceClientException : Exception
{
    public IntelligenceClientException(HttpStatusCode statusCode, string responseBody)
        : base($"Intelligence API request failed with status {(int)statusCode} ({statusCode}): {responseBody}")
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }

    public HttpStatusCode StatusCode { get; }

    public string ResponseBody { get; }
}
