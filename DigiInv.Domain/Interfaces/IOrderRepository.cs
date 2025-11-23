using DigiInv.Domain.Entities;

namespace DigiInv.Domain.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
    Task<(IEnumerable<Order> Items, int TotalCount)> GetOrdersByUserIdPagedAsync(int userId, int pageNumber, int pageSize);
}
