namespace Orders.Api.Contracts.Requests;

public class OrderRequest
{
    public Guid CustomerId { get; set; }
    
    public int? PaymentId { get; set; }
}