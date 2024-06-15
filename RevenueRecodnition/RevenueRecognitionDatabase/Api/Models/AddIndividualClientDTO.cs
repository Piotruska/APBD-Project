using System.ComponentModel.DataAnnotations;

namespace RevenueRecodnition.Api.Models;

public class AddCompanyClientDTO
{
    [Required]
    [MaxLength(200)]
    public string Address { get; set; }
    [Required]
    [MaxLength(200)]
    public string Email { get; set; }
    [Required]
    [MaxLength(9)]
    public string PhoneNumber { get; set; }
    [Required]
    [MaxLength(200)]
    public string CompanyName { get; set; }
    [Required]
    [MaxLength(200)]
    public string KRS { get; set; }
}