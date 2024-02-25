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

    public async Task<bool> DeleteAsync(Guid id)
    {
        var order = await FindAsync(id);
        appDbContext.Orders.Remove(order!);

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