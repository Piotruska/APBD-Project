using Microsoft.Extensions.Caching.Memory;

namespace RevenueRecodnition.DataBase.Entities;

public class IndividualClient
{
    public int IdClient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PESEL { get; set; }

    public virtual Client Client { get; set; }
    
}