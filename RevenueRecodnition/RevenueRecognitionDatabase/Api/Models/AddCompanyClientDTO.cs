using System.ComponentModel.DataAnnotations;

namespace RevenueRecodnition.Api.Models;

public class AddIndividualClientDTO
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
    [MaxLength(200)] // making it local
    public string FirstName { get; set; }
    [Required]
    [MaxLength(200)]
    public string LastName { get; set; }
    [Required]
    [MaxLength(11)]
    public string PESEL { get; set; }
}