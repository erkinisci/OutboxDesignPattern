namespace Orders.Api.Contracts.Response;

public class GetAllOrdersResponse
{
    public IEnumerable<OrderResponse> Orders { get; init; } = Enumerable.Empty<OrderResponse>();
}