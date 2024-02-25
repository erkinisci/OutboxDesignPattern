using MassTransit;
using Orders.Contracts;

namespace Orders.Processor.Consumers;

public class OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger) : IConsumer<OrderCreated>
{
    public Task Consume(ConsumeContext<OrderCreated> context)
    {
        var message = context.Message;
        
        logger.LogInformation(message.ToString());
        
        return Task.CompletedTask;
    }
}