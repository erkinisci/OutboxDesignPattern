using MassTransit;
using Orders.Contracts;

namespace Orders.Processor.Consumers;

public class OrderDeletedConsumer(ILogger<OrderDeletedConsumer> logger) : IConsumer<OrderDeleted>
{
    public Task Consume(ConsumeContext<OrderDeleted> context)
    {
        logger.LogInformation(context.Message.ToString());
        
        return Task.CompletedTask;
    }
}