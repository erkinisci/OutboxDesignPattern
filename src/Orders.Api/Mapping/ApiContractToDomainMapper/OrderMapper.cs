using Orders.Api.Contracts.Requests;
using Orders.Api.Domain;

namespace Orders.Api.Mapping.ApiContractToDomainMapper;

public static class OrderMapper
{
    public static Order ToOrder(this OrderRequest request)
    {
        return new Order
        {
            CustomerId = request.CustomerId,
            PaymentId = request.PaymentId
        };
    }
    
    public static Order ToOrder(this UpdateOrderRequest request)
    {
        return new Order
        {
            Id = request.Id,
            CustomerId = request.OrderRequest.CustomerId,
            PaymentId = request.OrderRequest.PaymentId
        };
    }
}