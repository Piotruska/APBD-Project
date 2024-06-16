using System.Runtime.InteropServices.JavaScript;

namespace RevenueRecodnition.DataBase.Entities;

public class Contract
{
    public int IdContract { get; set; }
    public DateTime StartDatePayement { get; set; }
    public DateTime EndDatePayement { get; set; }
    public DateTime StartDateContract { get; set; }
    public DateTime EndDateContract { get; set; }
    public DateTime StartDateSupport { get; set; }
    public DateTime EndDateSupport { get; set; }
    public decimal Price { get; set; }
    public bool IsSigned { get; set; }
    public int IdClient { get; set; }
    public int IdProduct { get; set; }

    public virtual Client Client { get; set; }
    public virtual ICollection<Payment> Payments { get; set; }
    public virtual Product Product { get; set; }
    
}