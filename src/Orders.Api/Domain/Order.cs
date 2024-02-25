namespace Orders.Api.Domain;

public class Order
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    public int? PaymentId { get; set; }
    public OrderStatus OrderStatus { get; init; } = OrderStatus.Submitted;
    
    public DateTime OrderDate { get; init; } = DateTime.UtcNow;
}