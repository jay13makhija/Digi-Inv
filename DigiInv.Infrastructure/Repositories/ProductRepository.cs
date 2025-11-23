using DigiInv.Domain.Entities;
using DigiInv.Domain.Interfaces;
using DigiInv.Infrastructure.Data;

namespace DigiInv.Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }
}
