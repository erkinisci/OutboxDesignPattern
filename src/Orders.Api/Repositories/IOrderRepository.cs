using Orders.Api.Domain;

namespace Orders.Api.Repositories;

public interface IOrderRepository
{
    Task<Order?> FindAsync(Guid id);
    Task CreateAsync(Order order);
    Task<bool> UpdateAsync(Order order);
    Task<bool> UpdateAsync<T>(Order order, Func<Order, Task<T>?> outboxAction,
        bool continueOnCapturedContext = false) where T : class;
    
    Task<bool> DeleteAsync(Guid id);
    Task<bool> DeleteAsync<T>(Guid id, Func<Guid, Task<T>?> outboxAction,
        bool continueOnCapturedContext = false) where T : class;
    Task<bool> SaveAsync(CancellationToken cancellationToken);
    Task<Order[]> GetAllAsync(CancellationToken cancellationToken);
}