using AutoMapper;
using DigiInv.Application.DTOs;
using DigiInv.Application.Interfaces;
using DigiInv.Application.Wrappers;
using DigiInv.Domain.Entities;
using DigiInv.Domain.Interfaces;

namespace DigiInv.Application.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;

    public OrderService(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _notificationService = notificationService;
    }

    public async Task<ApiResponse<OrderDto>> CreateOrderAsync(CreateOrderDto createOrderDto)
    {
        // 1. Validate Stock
        foreach (var item in createOrderDto.Items)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
            if (product == null)
                return new ApiResponse<OrderDto>($"Product with ID {item.ProductId} not found.");
            
            if (product.StockQuantity < item.Quantity)
                return new ApiResponse<OrderDto>($"Insufficient stock for product {product.Name}. Available: {product.StockQuantity}");
        }

        // 2. Create Order
        var order = new Order
        {
            UserId = createOrderDto.UserId,
            OrderDate = DateTime.UtcNow,
            Status = "Pending",
            ShippingAddress = "Default Address", // In real app, get from DTO
            OrderItems = new List<OrderItem>()
        };

        decimal totalAmount = 0;

        foreach (var itemDto in createOrderDto.Items)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(itemDto.ProductId);
            if (product != null)
            {
                // Deduct Stock
                product.StockQuantity -= itemDto.Quantity;
                _unitOfWork.Products.Update(product);

                var orderItem = new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price - product.DiscountAmount // Apply discount
                };
                order.OrderItems.Add(orderItem);
                totalAmount += orderItem.UnitPrice * orderItem.Quantity;
            }
        }

        order.TotalAmount = totalAmount;

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.CompleteAsync();

        // Send Notification
        var user = await _unitOfWork.Users.GetByIdAsync(order.UserId);
        if (user != null)
        {
            await _notificationService.SendOrderConfirmationAsync(user.Email, order.Id);
        }

        var orderDto = _mapper.Map<OrderDto>(order);
        return new ApiResponse<OrderDto>(orderDto, "Order created successfully");
    }

    public async Task<ApiResponse<OrderDto>> GetOrderByIdAsync(int id)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);
        if (order == null) return new ApiResponse<OrderDto>("Order not found");
        
        var dto = _mapper.Map<OrderDto>(order);
        return new ApiResponse<OrderDto>(dto);
    }

    public async Task<PagedResponse<IEnumerable<OrderDto>>> GetOrdersByUserIdAsync(int userId, int pageNumber, int pageSize)
    {
        var (orders, totalCount) = await _unitOfWork.Orders.GetOrdersByUserIdPagedAsync(userId, pageNumber, pageSize);
        var dtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
        return new PagedResponse<IEnumerable<OrderDto>>(dtos, pageNumber, pageSize, totalCount);
    }
}
