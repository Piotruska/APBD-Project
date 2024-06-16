namespace RevenueRecodnition.DataBase.Entities;

public class Product
{
    public int IdProduct { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CurrentVersion { get; set; }
    public string Category { get; set; }
    public decimal BasePrice { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; }
    public virtual ICollection<Subscription> Subscriptions { get; set; }
}