namespace Orders.Contracts;

public record OrderCreated(Guid Id, Guid CustomerId, int? PaymentId, int OrderStatus, DateTime OrderDate);