using MassTransit;
using Orders.Contracts;

namespace Orders.Processor.Consumers;

public class OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger) : IConsumer<OrderCreated>
{
    public Task Consume(ConsumeContext<OrderCreated> context)
    {
        logger.LogInformation(context.Message.ToString());
        
        return Task.CompletedTask;
    }
}