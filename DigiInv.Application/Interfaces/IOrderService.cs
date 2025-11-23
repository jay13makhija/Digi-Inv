using DigiInv.Application.DTOs;
using DigiInv.Application.Wrappers;

namespace DigiInv.Application.Interfaces;

public interface IOrderService
{
    Task<ApiResponse<OrderDto>> CreateOrderAsync(CreateOrderDto createOrderDto);
    Task<PagedResponse<IEnumerable<OrderDto>>> GetOrdersByUserIdAsync(int userId, int pageNumber, int pageSize);
    Task<ApiResponse<OrderDto>> GetOrderByIdAsync(int id);
}
