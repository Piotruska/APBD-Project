namespace RevenueRecodnition.Api.Exeptions;

public class UnauthorizedExeption : Exception
{
    public UnauthorizedExeption(string? message) : base(message)
    {
    }
}