using Orders.Api.Domain;

namespace Orders.Api.Services;

public interface IOrderService
{
    Task<bool> CreateAsync(Order order);
    Task<Order?> GetAsync(Guid id);
    Task<Order[]> GetAllAsync(CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Order order);
    Task<bool> DeleteAsync(Guid id);
}