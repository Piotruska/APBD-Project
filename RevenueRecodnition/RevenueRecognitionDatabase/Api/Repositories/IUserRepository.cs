using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Repositories;

public interface IUserRepository
{
    public Task AddNewUserAsync(User user);
    public Task<User?> GetUserAsync(string username);
}