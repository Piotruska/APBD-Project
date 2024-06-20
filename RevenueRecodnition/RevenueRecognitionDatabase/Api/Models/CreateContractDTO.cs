using System.ComponentModel.DataAnnotations;
using RevenueRecodnition.Api.Exeptions;

namespace RevenueRecodnition.Api.Models;

public class CreateContractDTO
{
    private int _timePeriodForPayement;
    
    [Required] 
    public int TimePeriodForPayement {
        get
        {
            return _timePeriodForPayement;
        }
        set
        {
            if (value < 3 || value > 30)
            {
                throw new BadRequestExeption("TimePeriod cannot be less then 3 or larger then 30 (Domain)");
            }

            _timePeriodForPayement = value;
        }
    }
    
    private int _contractLengthInYears;
    
    [Required] 
    public int ContractLengthInYears {
        get
        {
            return _contractLengthInYears;
        }
        set
        {
            if (value > 3 || value < 0)
            {
                throw new BadRequestExeption("Support can only be [0,1,2,3] years");
            }

            _contractLengthInYears = value; 
        }
        
    }
    [Required] 
    public int AdditionalSupportTimeInYears { get; set; }
    [Required]
    public int ClientId { get; set; }
    [Required]
    public int ProductId { get; set; }
    
}