using Asp.Versioning;
using DigiInv.Application.DTOs;
using DigiInv.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DigiInv.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderDto createOrderDto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null) return Unauthorized();

        if (int.Parse(userIdClaim) != createOrderDto.UserId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var response = await _orderService.CreateOrderAsync(createOrderDto);
        if (!response.Succeeded)
        {
            return BadRequest(response);
        }

        return CreatedAtAction(nameof(GetById), new { id = response.Data!.Id, version = "1" }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _orderService.GetOrderByIdAsync(id);
        if (!response.Succeeded || response.Data == null) return NotFound(response);

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim != null && int.Parse(userIdClaim) != response.Data.UserId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        return Ok(response);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(int userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim != null && int.Parse(userIdClaim) != userId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var response = await _orderService.GetOrdersByUserIdAsync(userId, pageNumber, pageSize);
        return Ok(response);
    }
}
