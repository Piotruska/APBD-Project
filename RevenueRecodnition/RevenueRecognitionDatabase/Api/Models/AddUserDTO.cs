using System.ComponentModel.DataAnnotations;

namespace RevenueRecodnition.Api.Models;

public class AddUserDTO
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Type { get; set; }
}