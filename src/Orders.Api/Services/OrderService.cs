using FluentValidation;
using FluentValidation.Results;
using MassTransit;
using Orders.Api.Domain;
using Orders.Api.Repositories;
using Orders.Contracts;

namespace Orders.Api.Services;

public class OrderService(IOrderRepository orderRepository, IPublishEndpoint publishEndpoint) : IOrderService
{
    public async Task<bool> CreateAsync(Order order)
    {
        var findOrder = await orderRepository.FindAsync(order.Id);
        
        if (findOrder is not null)
        {
            var errorMessage = $"An order with id {order.Id} already exists";
            throw new ValidationException(errorMessage, GenerateValidationError(errorMessage));
        }

        await orderRepository.CreateAsync(order);
        
        var message = new OrderCreated(order.Id, order.CustomerId, order.PaymentId, (int)order.OrderStatus, order.OrderDate);
        await publishEndpoint.Publish(message);
        
        return await orderRepository.SaveAsync(CancellationToken.None);
    }

    public Task<Order?> GetAsync(Guid id)
    {
        return orderRepository.FindAsync(id);
    }
    
    public async Task<Order[]> GetAllAsync(CancellationToken cancellationToken)
    {
        return await orderRepository.GetAllAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(Order order)
    {
        var findOrder = await orderRepository.FindAsync(order.Id);
        
        if (findOrder is null)
        {
            var errorMessage = $"An order with id {order.Id} doest not exists";
            throw new ValidationException(errorMessage, GenerateValidationError(errorMessage));
        }
        
        return await orderRepository.UpdateAsync(order, async _ =>
        {
            var message = new OrderUpdated(order.Id, order.CustomerId, order.PaymentId, (int)order.OrderStatus, order.OrderDate);
            await publishEndpoint.Publish(message);
            return message;
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var findOrder = await orderRepository.FindAsync(id);

        if (findOrder is null)
        {
            var errorMessage = $"An order with id {id} does not exists";
            throw new ValidationException(errorMessage, GenerateValidationError(errorMessage));
        }
        
        return await orderRepository.DeleteAsync(id, async _ =>
        {
            var message = new OrderDeleted(id);
            await publishEndpoint.Publish(message);
            return message;
        });
    }
    
    private static ValidationFailure[] GenerateValidationError(string message)
    {
        return
        [
            new ValidationFailure(nameof(Order), message)
        ];
    }
}