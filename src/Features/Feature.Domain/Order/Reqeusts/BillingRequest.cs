namespace Feature.Domain.Order.Reqeusts;

public class BillingRequest
{
    public string OrderId { get; set; }
    public string CustomerNo { get; set; }
    public double Discount { get; set; }
    public double Tax { get; set; }
    public double Total { get; set; }
}