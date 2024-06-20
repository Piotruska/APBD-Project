using System.ComponentModel.DataAnnotations;

namespace RevenueRecodnition.Api.Models;

public class RevenueCalculationRequestDTO
{
    [Required]
    public string For { get; set; } 
    [Required]
    public int ProductId { get; set; } 
    [Required]
    public string CurrencyCode { get; set; } 
}