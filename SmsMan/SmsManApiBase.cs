using System.Text.Json;
using SmsMan.Helpers;

namespace SmsMan;

public abstract class SmsManApiBase
{
    private readonly HttpClient _httpClient;
    private readonly string _token;

    protected SmsManApiBase(HttpClient httpClient, string token)
    {
        _httpClient = httpClient;
        _token = token;
    }
    
    protected async Task<JsonDocument> GetAsync(string path, Dictionary<string, string>? queryString = null)
    {
        queryString ??= new Dictionary<string, string>();
        queryString["token"] = _token;

        var response =
            await _httpClient.GetAsync($"https://api.sms-man.com/{QueryHelpers.AddQueryString(path, queryString!)}");
        await using var stream = await response.Content.ReadAsStreamAsync();
        var jsonDocument = await JsonDocument.ParseAsync(stream);
        var rootElement = jsonDocument.RootElement;
        if (rootElement.TryGetProperty("error_code", out var errorCodeProp) &&
            rootElement.TryGetProperty("error_msg", out var errorMsgProp))
        {
            throw new SmsManException(errorCodeProp.GetString()!, errorMsgProp.GetString()!);
        }

        return jsonDocument;
    }

    protected async Task<T> GetAndDeserializeAsync<T>(string path, Dictionary<string, string>? queryString = null)
    {
        var jsonDocument = await GetAsync(path, queryString);
        return jsonDocument.Deserialize<T>()!;
    }
}