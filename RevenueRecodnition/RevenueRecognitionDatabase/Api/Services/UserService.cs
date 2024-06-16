using RevenueRecodnition.Api.Exeptions;
using RevenueRecodnition.Api.Helpers;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Repositories;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Services;

public class UserService : IUserService
{
    private IUserRepository _userRepository;

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

    private void EnsureUsernameDoesNotExist(User? user)
    {
        if (user != null)
        {
            throw new BadRequestExeption($"User with username '{user.Username}' already exists.");
        }
    }
}