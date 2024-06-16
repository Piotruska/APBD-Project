using RevenueRecodnition.Api.Models;

namespace RevenueRecodnition.Api.Services;

public interface IRevenueService
{
    public Task<decimal> CalculateCurrentRevenueAsync(RevenueCalculationRequestDTO requestDto);
    
    public Task<decimal> CalculatePredictedRevenueAsync(RevenueCalculationRequestDTO requestDto);
}