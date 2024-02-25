using Microsoft.EntityFrameworkCore;
using Orders.Api.Data;
using Orders.Api.Domain;

namespace Orders.Api.Repositories;

public class OrderRepository(AppDbContext appDbContext) : IOrderRepository
{
    public async Task<Order?> FindAsync(Guid id)
    {
        return await appDbContext.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task CreateAsync(Order order)
    {
        await appDbContext.Orders.AddAsync(order);
    }

    public async Task<bool> UpdateAsync(Order order)
    {
        appDbContext.Orders.Update(order);
        
        var result = await appDbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> UpdateAsync<T>(Order order, Func<Order, Task<T>?> outboxAction, bool continueOnCapturedContext = false) where T : class
    {
        appDbContext.Orders.Update(order);
        
        await outboxAction(order)!.ConfigureAwait(continueOnCapturedContext);
        
        var result = await appDbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var order = await FindAsync(id);
        appDbContext.Orders.Remove(order!);

        var result = await appDbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteAsync<T>(Guid id, Func<Guid, Task<T>?> outboxAction, bool continueOnCapturedContext = false) where T : class
    {
        var order = await FindAsync(id);
        appDbContext.Orders.Remove(order!);
        
        await outboxAction(id)!.ConfigureAwait(continueOnCapturedContext);

        var result = await appDbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> SaveAsync(CancellationToken cancellationToken)
    {
        var result = await appDbContext.SaveChangesAsync(cancellationToken);
        return result > 0;
    }

    public async Task<Order[]> GetAllAsync(CancellationToken cancellationToken)
    {
        return await appDbContext.Orders.ToArrayAsync(cancellationToken);
    }
}