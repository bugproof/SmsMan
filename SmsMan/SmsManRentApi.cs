using System.Globalization;

namespace SmsMan;

/// <summary>
/// Implements SMS-MAN Rent API (https://sms-man.com/site/rent-api)
/// </summary>
public class SmsManRentApi : SmsManApiBase
{
    public SmsManRentApi(HttpClient httpClient, string token) : base(httpClient, token)
    {
    }

    /// <summary>
    /// Gets account balance in USD
    /// </summary>
    /// <returns></returns>
    public async Task<decimal> GetBalance()
    {
        var jsonDocument = await GetAsync("rent-api/get-balance");
        return decimal.Parse(jsonDocument.RootElement.GetProperty("balance").GetString()!,
            CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets available countries where you can rent phone numbers
    /// </summary>
    /// <param name="type">hour/day/week/month</param>
    /// <param name="time">Rental time</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task GetAvailableCountriesForRent(string type, int time)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Requests number for rent
    /// </summary>
    /// <param name="countryId"></param>
    /// <param name="type">hour/day/week/month</param>
    /// <param name="time">Rental time</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task RequestNumberForRent(int countryId, string type, int time)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Sets a new status for the specified request
    /// </summary>
    /// <param name="requestId"></param>
    /// <param name="status"></param>
    public async Task SetRequestStatus(int requestId, string status)
    {
        await GetAsync($"rent-api/set-status?request_id={requestId}&status={status}");
    }

    /// <summary>
    /// Gets the latest SMS
    /// </summary>
    /// <param name="requestId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task GetLatestSms(int requestId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets all SMS messages for number associated with request id
    /// </summary>
    /// <param name="requestId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task GetAllSms(int requestId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets all previously requested numbers
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task GetAllRequestedNumbers()
    {
        throw new NotImplementedException();
    }
}