namespace RevenueRecodnition.DataBase.Entities;

public class Subscription
{
    public int IdSubscription { get; set; }
    public string Name { get; set; }
    public string RenewalPeriod { get; set; }
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int IdClient { get; set; }
    public int IdProduct { get; set; }

    public Product Product { get; set; }
    public ICollection<Payment> Payments { get; set; }
    public Client Client { get; set; }
    
}