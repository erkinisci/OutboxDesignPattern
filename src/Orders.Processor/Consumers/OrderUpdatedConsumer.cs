using MassTransit;
using Orders.Contracts;

namespace Orders.Processor.Consumers;

public class OrderUpdatedConsumer(ILogger<OrderUpdatedConsumer> logger) : IConsumer<OrderUpdated>
{
    public Task Consume(ConsumeContext<OrderUpdated> context)
    {
        logger.LogInformation(context.Message.ToString());
        
        return Task.CompletedTask;
    }
}