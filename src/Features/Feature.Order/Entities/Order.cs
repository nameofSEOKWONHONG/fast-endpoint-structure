using Infrastructure.Entity;

namespace Feature.Order.Entities;

public class Order : EntityBase
{
    public string OrderId { get; set; }
    public string OrderGuid { get; set; }
}