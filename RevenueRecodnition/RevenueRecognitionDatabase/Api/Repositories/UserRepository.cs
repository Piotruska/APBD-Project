using Microsoft.EntityFrameworkCore;
using RevenueRecodnition.DataBase.Context;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Repositories;

public class UserRepository : IUserRepository
{
    private RRConext _context;

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
}