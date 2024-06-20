namespace RevenueRecodnition.Api.Services.Interfaces;

public interface IExchangeRateService
{
    public Task<decimal> GetExchangeRateAsync(string currencyCode);
}