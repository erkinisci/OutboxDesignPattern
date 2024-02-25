using Microsoft.AspNetCore.Mvc;
using Orders.Api.Attributes;
using Orders.Api.Contracts.Requests;
using Orders.Api.Mapping.ApiContractToDomainMapper;
using Orders.Api.Mapping.DomainToApiContractMapper;
using Orders.Api.Services;

namespace Orders.Api.Controllers;

[ApiController]
public class OrderController(IOrderService orderService) : ControllerBase
{
    [HttpPost("orders")]
    public async Task<IActionResult> Create([FromBody] OrderRequest request)
    {
        var order = request.ToOrder();

        await orderService.CreateAsync(order);

        var response = order.ToOrderResponse();

        return CreatedAtAction("Get", new { response.Id }, response);
    }

    [HttpGet("orders/{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var order = await orderService.GetAsync(id);

        if (order is null)
        {
            return NotFound();
        }

        var customerResponse = order.ToOrderResponse();
        return Ok(customerResponse);
    }
    
    [HttpGet("orders")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var orders = await orderService.GetAllAsync(cancellationToken);
        var ordersResponse = orders.ToOrdersResponse();
        
        return Ok(ordersResponse);
    }
    
    [HttpPut("orders/{id:guid}")]
    public async Task<IActionResult> Update([FromMultiSource] UpdateOrderRequest request)
    {
        var order = request.ToOrder();
        await orderService.UpdateAsync(order);

        var customerResponse = order.ToOrderResponse();
        return Ok(customerResponse);
    }
    
    [HttpDelete("orders/{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await orderService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
}