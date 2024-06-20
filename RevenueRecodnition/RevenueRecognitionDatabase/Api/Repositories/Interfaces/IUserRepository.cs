using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Repositories.Interfaces;

public interface IUserRepository
{
    public Task AddNewUserAsync(User user);
    public Task<User?> GetUserAsync(string username);
    public Task<User?> GetUserAsync(int userId);
    public Task DeleteUserAsync(User user);
}