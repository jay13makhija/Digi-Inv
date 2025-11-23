using DigiInv.Domain.Entities;
using DigiInv.Domain.Interfaces;
using DigiInv.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DigiInv.Infrastructure.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Where(o => o.UserId == userId)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Order> Items, int TotalCount)> GetOrdersByUserIdPagedAsync(int userId, int pageNumber, int pageSize)
    {
        var query = _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Where(o => o.UserId == userId);

        var totalCount = await query.CountAsync();
        
        var items = await query
            .OrderByDescending(o => o.OrderDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
