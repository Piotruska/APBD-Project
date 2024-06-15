using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RevenueRecodnition.DataBase.Entities;

public class Client
{
    public int IdClient { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public virtual IndividualClient? IndividualClient { get; set; }
    public virtual CompanyClient? CompanyClient { get; set; }
    public virtual ICollection<Contract> Contracts { get; set; }
    public virtual ICollection<Subscription> Subscriptions { get; set; }
}