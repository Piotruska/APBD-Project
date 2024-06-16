namespace RevenueRecodnition.DataBase.Entities;

public class Payment
{
    public int IdPayement { get; set; }
    public int? IdContract { get; set; }
    public int? IdSubscription { get; set; }
    public decimal Amount { get; set; }
    public DateTime DatePayed { get; set; }
    public virtual Contract? Contract { get; set; }
    public virtual Subscription? Subscription { get; set; }
    
}