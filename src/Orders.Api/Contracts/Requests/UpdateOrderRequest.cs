using Microsoft.AspNetCore.Mvc;

namespace Orders.Api.Contracts.Requests;

public class UpdateOrderRequest
{
    [FromRoute(Name = "id")] public Guid Id { get; init; }

    [FromBody] public OrderRequest OrderRequest { get; set; } = default!;
}