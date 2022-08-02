using System.Globalization;

namespace SmsMan;

/// <summary>
/// Implements SMS-MAN JSON API (https://sms-man.com/site/docs-apiv2)
/// </summary>
public class SmsManApiV2 : SmsManApiBase
{
    public SmsManApiV2(HttpClient httpClient, string token) : base(httpClient, token)
    {
    }
    
    /// <summary>
    /// Gets account balance in USD
    /// </summary>
    /// <returns></returns>
    public async Task<decimal> GetBalance()
    {
        var jsonDocument = await GetAsync("control/get-balance");
        return decimal.Parse(jsonDocument.RootElement.GetProperty("balance").GetString()!,
            CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Requests a phone number. To poll for SMS message use GetSms method.
    /// </summary>
    /// <param name="countryId"></param>
    /// <param name="applicationId"></param>
    /// <param name="referralId"></param>
    /// <returns></returns>
    public Task<GetNumberResponse> RequestNumber(int? countryId, int? applicationId, string? referralId)
    {
        var queryString = new Dictionary<string, string>();

        if (countryId.HasValue)
        {
            queryString["country_id"] = countryId.Value.ToString();
        }

        if (applicationId.HasValue)
        {
            queryString["application_id"] = applicationId.Value.ToString();
        }

        if (!string.IsNullOrWhiteSpace(referralId))
        {
            queryString["ref"] = referralId;
        }

        return GetAndDeserializeAsync<GetNumberResponse>("control/get-number", queryString);
    }

    /// <summary>
    /// Gets SMS message if not received throws and error_code = "wait_sms"
    /// </summary>
    /// <param name="requestId"></param>
    /// <returns></returns>
    public Task<GetSmsResponse> GetSms(int requestId)
        => GetAndDeserializeAsync<GetSmsResponse>($"control/get-sms?request_id={requestId}");
    
    /// <summary>
    /// Waits for an SMS message
    /// </summary>
    /// <param name="requestId"></param>
    /// <param name="interval">(optional) default is 5 seconds</param>
    /// <returns></returns>
    public async Task<GetSmsResponse> PollSms(int requestId, TimeSpan? interval = null)
    {
        interval ??= TimeSpan.FromSeconds(5);
        
        // Delay first because SMS messages don't come that fast
        await Task.Delay(interval.Value);
        
        try
        {
            return await GetSms(requestId);
        }
        catch (SmsManException exception) when (exception.ErrorCode == "wait_sms")
        {
            return await PollSms(requestId, interval);
        }
    }

    /// <summary>
    /// Sets a new status for the specified request
    /// </summary>
    /// <param name="requestId"></param>
    /// <param name="status"></param>
    public async Task SetRequestStatus(int requestId, string status)
    {
        await GetAsync($"control/set-status?request_id={requestId}&status={status}");
    }

    /// <summary>
    /// Gets cost and count grouped by application id
    /// </summary>
    /// <param name="countryId"></param>
    /// <param name="applicationId"></param>
    /// <returns>Dictionary where key is application id and value contains cost and available numbers for that application</returns>
    public Task<Dictionary<string, CostAndCount>> GetNumbersCostAndAvailableCountByCountry(int countryId,
        int? applicationId) =>
        GetAndDeserializeAsync<Dictionary<string, CostAndCount>>(
            $"control/get-prices?country_id={countryId}{(applicationId.HasValue ? $"&application_id={applicationId.Value}" : "")}");

    /// <summary>
    /// Get cost and count grouped by country id and then by application id
    /// </summary>
    /// <returns>Dictionary where key is country id and value is another dictionary where key = application id and value is cost and numbers count</returns>
    public Task<Dictionary<string, Dictionary<string, CostAndCount>>> GetNumbersCostAndAvailableCount() =>
        GetAndDeserializeAsync<Dictionary<string, Dictionary<string, CostAndCount>>>("control/get-prices");

    /// <summary>
    /// Gets all countries
    /// </summary>
    /// <returns>Dictionary where key is country id and value contains details</returns>
    public Task<Dictionary<string, Country>> GetAllCountries() =>
        GetAndDeserializeAsync<Dictionary<string, Country>>("control/countries");

    /// <summary>
    /// Gets all supported services (applications) e.g. Instagram, Discord etc.
    /// </summary>
    /// <returns>Dictionary where key is service id and value contains details</returns>
    public Task<Dictionary<string, Service>> GetAllServices() =>
        GetAndDeserializeAsync<Dictionary<string, Service>>("control/applications");
}