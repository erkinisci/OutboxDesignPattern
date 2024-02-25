using Orders.Api.Domain;

namespace Orders.Api.Contracts.Response;

public class OrderResponse
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; set; }
    public int? PaymentId { get; set; }
    public OrderStatus OrderStatus { get; init; }
    public DateTime OrderDate { get; set; }
}