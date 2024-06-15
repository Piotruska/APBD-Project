using System.Runtime.InteropServices.JavaScript;

namespace RevenueRecodnition.DataBase.Entities;

public class Contract
{
    public int IdContract { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public double Price { get; set; }
    public bool IsSigned { get; set; }
    public int IdClient { get; set; }
    public int IdProduct { get; set; }

    public Client Client { get; set; }
    public ICollection<Payment> Payments { get; set; }
    public Product Product { get; set; }
    
}