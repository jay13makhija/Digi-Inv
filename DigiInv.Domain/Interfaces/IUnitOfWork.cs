namespace DigiInv.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    IOrderRepository Orders { get; }
    IUserRepository Users { get; }
    Task<int> CompleteAsync();
}
