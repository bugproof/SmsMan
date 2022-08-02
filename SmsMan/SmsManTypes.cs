using System.Text.Json.Serialization;
using SmsMan.JsonConverters;

namespace SmsMan;

public class GetLimitsResponse
{
    [JsonPropertyName("application_id")]
    public int ApplicationId { get; set; }
    
    [JsonPropertyName("country_id")]
    public int CountryId { get; set; }
    
    [JsonPropertyName("numbers")]
    public int Numbers { get; set; }
}

public class GetNumberResponse
{
    [JsonPropertyName("request_id")]
    public int RequestId { get; set; }
    
    [JsonPropertyName("country_id")]
    public int CountryId { get; set; }
    
    [JsonPropertyName("application_id")]
    public int ApplicationId { get; set; }

    [JsonPropertyName("number")]
    public string Number { get; set; }
}

public class GetSmsResponse : GetNumberResponse
{
    [JsonPropertyName("sms_code")]
    public string SmsCode { get; set; }
}

public class Country
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; }
}

public class Service
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("code")]
    public string Code { get; set; }
}

public class CostAndCount
{
    [JsonPropertyName("cost")]
    [JsonConverter(typeof(StringToDecimalInvariantCultureConverter))]
    public decimal Cost { get; set; }
    
    [JsonPropertyName("count")]
    public int Count { get; set; }
}