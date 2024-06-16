namespace RevenueRecodnition.Api.Services;

public interface IExchangeRateService
{
    public Task<decimal> GetExchangeRateAsync(string currencyCode);
}