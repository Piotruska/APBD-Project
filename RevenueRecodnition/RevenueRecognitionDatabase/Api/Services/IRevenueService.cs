using RevenueRecodnition.Api.Models;

namespace RevenueRecodnition.Api.Services;

public interface IRevenueService
{
    public Task<decimal> CalculateCurrentRevenueAsync(RevenueCalculationRequest request);
    
    public Task<decimal> CalculatePredictedRevenueAsync(RevenueCalculationRequest request);
}