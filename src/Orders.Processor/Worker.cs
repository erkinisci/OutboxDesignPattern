using MassTransit;

namespace Orders.Processor;

public class Worker(ILogger<Worker> logger, IBus bus) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await bus.Publish("Order.Processor has stopped", stoppingToken);
            
            await Task.Delay(1000, stoppingToken);
        }
    }
}