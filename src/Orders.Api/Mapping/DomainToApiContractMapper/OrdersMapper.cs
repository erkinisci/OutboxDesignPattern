using Orders.Api.Contracts.Response;
using Orders.Api.Domain;

namespace Orders.Api.Mapping.DomainToApiContractMapper;

public static class OrdersMapper
{
    public static GetAllOrdersResponse ToOrdersResponse(this IEnumerable<Order> orders)
    {
        return new GetAllOrdersResponse
        {
            Orders = orders.Select(x => new OrderResponse
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                PaymentId = x.PaymentId,
                OrderDate = x.OrderDate,
                OrderStatus = x.OrderStatus
            })
        };
    }
}