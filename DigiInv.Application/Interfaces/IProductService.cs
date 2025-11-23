using DigiInv.Application.DTOs;
using DigiInv.Application.Wrappers;

namespace DigiInv.Application.Interfaces;

public interface IProductService
{
    Task<PagedResponse<IEnumerable<ProductDto>>> GetProductsAsync(int pageNumber, int pageSize, string? category = null);
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
    Task UpdateProductAsync(int id, CreateProductDto createProductDto);
    Task DeleteProductAsync(int id);
}
