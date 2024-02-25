using Orders.Api.Domain;

namespace Orders.Api.Repositories;

public interface IOrderRepository
{
    Task<Order?> FindAsync(Guid id);
    Task CreateAsync(Order order);
    Task<bool> UpdateAsync(Order order);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> SaveAsync(CancellationToken cancellationToken);
    Task<Order[]> GetAllAsync(CancellationToken cancellationToken);
}