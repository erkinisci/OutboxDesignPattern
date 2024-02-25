using MassTransit;
using Orders.Contracts;

namespace Orders.Processor.Consumers;

public class OrderUpdatedConsumer(ILogger<OrderUpdatedConsumer> logger) : IConsumer<OrderUpdated>
{
    public Task Consume(ConsumeContext<OrderUpdated> context)
    {
        var message = context.Message;
        logger.LogInformation(message.ToString());
        
        return Task.CompletedTask;
    }
}