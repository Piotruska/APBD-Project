namespace RevenueRecodnition.DataBase.Entities;

public class CompanyClient
{
    public int IdClient { get; set; }
    public string ComapnyName { get; set; }
    public string KRS { get; set; }
    
    public virtual Client Client { get; set; }
}