namespace RevenueRecodnition.Api.Exeptions;

public class NotFoundExeption : Exception
{
    public NotFoundExeption(string? message) : base(message)
    {
    }
}