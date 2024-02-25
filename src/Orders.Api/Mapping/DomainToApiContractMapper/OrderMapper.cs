using Orders.Api.Contracts.Response;
using Orders.Api.Domain;

namespace Orders.Api.Mapping.DomainToApiContractMapper;

public static class OrderMapper
{
    public static OrderResponse ToOrderResponse(this Order order)
    {
        return new OrderResponse
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            PaymentId = order.PaymentId,
            OrderStatus = order.OrderStatus,
            OrderDate = order.OrderDate
        };
    }
}