using System.ComponentModel.DataAnnotations;

namespace RevenueRecodnition.Api.Models;

public class UpdateIndividualClientDTO
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
    public string FirstName { get; set; }
    [Required]
    [MaxLength(200)]
    public string LastName { get; set; }
}