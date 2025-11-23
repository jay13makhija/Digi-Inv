using DigiInv.Domain.Interfaces;
using DigiInv.Infrastructure.Data;

namespace DigiInv.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IProductRepository Products { get; private set; }
    public IOrderRepository Orders { get; private set; }
    public IUserRepository Users { get; private set; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Products = new ProductRepository(_context);
        Orders = new OrderRepository(_context);
        Users = new UserRepository(_context);
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
