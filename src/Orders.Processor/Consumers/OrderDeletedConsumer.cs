using MassTransit;
using Orders.Contracts;

namespace Orders.Processor.Consumers;

public class OrderDeletedConsumer(ILogger<OrderDeletedConsumer> logger) : IConsumer<OrderDeleted>
{
    public Task Consume(ConsumeContext<OrderDeleted> context)
    {
        var message = context.Message;

        logger.LogInformation(message.ToString());
        
        return Task.CompletedTask;
    }
}