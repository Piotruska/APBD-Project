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

    
}