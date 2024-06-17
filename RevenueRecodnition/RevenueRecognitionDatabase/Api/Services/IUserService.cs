using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using RevenueRecodnition.Api.Models;

namespace RevenueRecodnition.Api.Services;

public interface IUserService
{
    public Task AddNewUserAsync(AddUserDTO dto);
    public Task RemoveUserAync(string username);
}