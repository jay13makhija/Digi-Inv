using DigiInv.Domain.Entities;
using DigiInv.Domain.Interfaces;
using DigiInv.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DigiInv.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}
