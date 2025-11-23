using DigiInv.Domain.Entities;

namespace DigiInv.Domain.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}
