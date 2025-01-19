namespace Feature.Domain.Order.Reqeusts;

public class OrderItemRequest
{
    public string ItemId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
}
