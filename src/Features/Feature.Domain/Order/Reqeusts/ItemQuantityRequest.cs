namespace Feature.Domain.Order.Reqeusts;

public class ItemQuantityRequest
{
    public string ItemId { get; set; }
    public int Quantity { get; set; }
}