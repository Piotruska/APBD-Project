using Microsoft.EntityFrameworkCore;
using RevenueRecodnition.DataBase.Context;
using RevenueRecodnition.DataBase.Entities;
using RevenueRecodnition.Api.Repositories.Interfaces;

namespace RevenueRecodnition.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly RRConext _context;

    public UserRepository(RRConext context)
    {
        _context = context;
    }

    public async Task AddNewUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetUserAsync(string username)
    {
        return await _context.Users.Where(x => x.Username == username).FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserAsync(int userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    public async Task DeleteUserAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
    
}