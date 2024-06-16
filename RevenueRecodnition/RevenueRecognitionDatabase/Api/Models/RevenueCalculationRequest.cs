namespace RevenueRecodnition.Api.Models;

public class RevenueCalculationRequest
{
    public string For { get; set; } 
    public int ProductId { get; set; } 
    public string CurrencyCode { get; set; } 
}