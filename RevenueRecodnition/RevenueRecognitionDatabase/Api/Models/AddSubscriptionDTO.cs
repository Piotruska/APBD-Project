using System.ComponentModel.DataAnnotations;

namespace RevenueRecodnition.Api.Models;

public class AddSubscriptionDTO
{
    [Required]
    public int IdClient { get; set; }
    [Required]
    public int IdProduct { get; set; }
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }
    [Required]
    public int RenewalPeriodInMonths { get; set; }
}