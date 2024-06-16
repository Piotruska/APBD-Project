using System.ComponentModel.DataAnnotations;

namespace RevenueRecodnition.Api.Models;

public class CreateContractDTO
{
    [Required] 
    public int TimePeriodForPayement { get; set; }
    [Required] 
    public int ContractLengthInYears { get; set; }
    [Required] 
    public int AdditionalSupportTimeInYears { get; set; }
    [Required]
    public int ClientId { get; set; }
    [Required]
    public int ProductId { get; set; }
}