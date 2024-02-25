namespace Orders.Contracts;

public record OrderUpdated(Guid Id, Guid CustomerId, int? PaymentId, int OrderStatus, DateTime OrderDate);