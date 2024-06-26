using RevenueRecodnition.Api.Exeptions;
using RevenueRecodnition.Api.Helpers;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Repositories.Interfaces;
using RevenueRecodnition.Api.Services.Interfaces;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task AddNewUserAsync(AddUserDTO dto)
    {
        var user = await _userRepository.GetUserAsync(dto.Username);
        EnsureUsernameDoesNotExist(user);
        
        if (dto.Type != "Admin" && dto.Type != "User")
        {
            throw new BadRequestExeption("Wrong user type. Type : [Admin , User]");
        }

        var userToAdd = new User()
        {
            Username = dto.Username,
            Password = PasswordHasher.HashPassword(dto.Password),
            Type = dto.Type
        };

        await _userRepository.AddNewUserAsync(userToAdd);
    }

    public async Task RemoveUserAync(int userId)
    {
        var user = await _userRepository.GetUserAsync(userId);
        EnsureUserExists(user);
        await _userRepository.DeleteUserAsync(user);
    }
    private void EnsureUserExists(User? user)
    {
        if (user == null)
        {
            throw new BadRequestExeption($"User dose not exists.");
        }
    }
    private void EnsureUsernameDoesNotExist(User? user)
    {
        if (user != null)
        {
            throw new BadRequestExeption($"User with username '{user.Username}' already exists.");
        }
    }
}