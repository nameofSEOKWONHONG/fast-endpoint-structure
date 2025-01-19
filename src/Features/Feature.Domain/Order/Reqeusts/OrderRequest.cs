namespace Feature.Domain.Order.Reqeusts;

public class OrderRequest
{
    public string OrderGuid { get; set; }
    public OrderItemRequest[] OrderItems { get; set; }
}
