using System.ComponentModel.DataAnnotations;

namespace RevenueRecodnition.Api.Modls;

public class UpdateCompanyClientDto
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
}