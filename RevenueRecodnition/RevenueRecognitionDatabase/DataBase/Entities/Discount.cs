namespace RevenueRecodnition.DataBase.Entities;

public class Discount
{
    public int IdDiscount { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Percentage { get; set; }
    
    
}