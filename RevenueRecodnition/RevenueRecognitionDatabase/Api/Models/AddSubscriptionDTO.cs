using System.ComponentModel.DataAnnotations;
using RevenueRecodnition.Api.Exeptions;

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

    private int _renewalPeriodInMonths;
    
    [Required]
    public int RenewalPeriodInMonths { 
        get
        {
            return _renewalPeriodInMonths;
        }
        set
        {
            if (value<1 || value>24)
            {
                throw new BadRequestExeption("Renewal period can only be from 1 - 24 months");
            }

            _renewalPeriodInMonths = value; 
        }}
}