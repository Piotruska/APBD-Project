namespace RevenueRecodnition.DataBase.Entities;

public class Subscription
{
    public int IdSubscription { get; set; }
    public string Name { get; set; }
    public string RenewalPeriod { get; set; }
    public decimal Price { get; set; }
    public DateTime StartDateRenewalPayement { get; set; }
    public DateTime EndDateRenewalPayement { get; set; }
    public int IdClient { get; set; }
    public int IdProduct { get; set; }
    public bool Canceled { get; set; }

    public virtual Product Product { get; set; }
    public virtual ICollection<Payment> Payments { get; set; }
    public virtual Client Client { get; set; }
    
}