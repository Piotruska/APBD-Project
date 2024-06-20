using Newtonsoft.Json;
using RevenueRecodnition.Api.Exeptions;
using RevenueRecodnition.Api.Services.Interfaces;

namespace RevenueRecodnition.Api.Services;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

public class ExchangeRateService : IExchangeRateService
{
    private readonly HttpClient _httpClient;

    public ExchangeRateService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal> GetExchangeRateAsync(string currencyCode)
    {
        string apiKey = "1d205dc964465792fc74be72"; 
        string requestUri = $"https://v6.exchangerate-api.com/v6/{apiKey}/latest/PLN";

        var response = await _httpClient.GetAsync(requestUri);
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<ExchangeRateApiResponse>(responseContent);
            if (data.ConversionRates != null && data.ConversionRates.TryGetValue(currencyCode, out decimal rate))
            {
                return rate;
            }
        }
        throw new BadRequestExeption("Exchange rate not found.");
    }
}

public class ExchangeRateApiResponse
{
    [JsonProperty("conversion_rates")]
    public Dictionary<string, decimal> ConversionRates { get; set; }
}

