using System.ComponentModel.DataAnnotations;

namespace RevenueRecodnition.Api.Models;

public class PaymentForContractDTO
{
    [Required]
    public int contractID { get; set; }
    [Required]
    public decimal Amount { get; set; }
}