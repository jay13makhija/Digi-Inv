using AutoMapper;
using DigiInv.Application.DTOs;
using DigiInv.Application.Interfaces;
using DigiInv.Application.Wrappers;
using DigiInv.Domain.Entities;
using DigiInv.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;

namespace DigiInv.Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly Microsoft.Extensions.Caching.Memory.IMemoryCache _cache;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper, Microsoft.Extensions.Caching.Memory.IMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
    {
        var product = _mapper.Map<Product>(createProductDto);
        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.CompleteAsync();
        return _mapper.Map<ProductDto>(product);
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product != null)
        {
            _unitOfWork.Products.Remove(product);
            await _unitOfWork.CompleteAsync();
        }
    }

    public async Task<PagedResponse<IEnumerable<ProductDto>>> GetProductsAsync(int pageNumber, int pageSize, string? category = null)
    {
        var cacheKey = $"products_{pageNumber}_{pageSize}_{category}";
        if (_cache.TryGetValue(cacheKey, out PagedResponse<IEnumerable<ProductDto>> cachedResponse))
        {
            return cachedResponse;
        }

        Expression<Func<Product, bool>>? filter = null;
        if (!string.IsNullOrEmpty(category))
        {
            filter = p => p.Category == category;
        }

        var (items, totalCount) = await _unitOfWork.Products.GetPagedAsync(pageNumber, pageSize, filter);
        var dtos = _mapper.Map<IEnumerable<ProductDto>>(items);

        var response = new PagedResponse<IEnumerable<ProductDto>>(dtos, pageNumber, pageSize, totalCount);
        
        var cacheOptions = new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

        _cache.Set(cacheKey, response, cacheOptions);

        return response;
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        return product == null ? null : _mapper.Map<ProductDto>(product);
    }

    public async Task UpdateProductAsync(int id, CreateProductDto createProductDto)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product != null)
        {
            _mapper.Map(createProductDto, product);
            _unitOfWork.Products.Update(product);
            await _unitOfWork.CompleteAsync();
        }
    }
}
