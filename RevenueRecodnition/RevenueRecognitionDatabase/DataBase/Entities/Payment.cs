namespace RevenueRecodnition.DataBase.Entities;

public class Payment
{
    public int IdPayement { get; set; }
    public int IdContract { get; set; }
    public int IdSubscription { get; set; }
    public DateTime DatePayed { get; set; }

    public Contract? Contract { get; set; }
    public Subscription? Subscription { get; set; }
    
}