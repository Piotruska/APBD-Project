using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using RevenueRecodnition.Api.Models;

namespace RevenueRecodnition.Api.Services.Interfaces;

public interface IUserService
{
    public Task AddNewUserAsync(AddUserDTO dto);
    public Task RemoveUserAync(int userId);
}