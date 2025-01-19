namespace Feature.Domain.Product.Requests;

public class PlanDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string Step { get; set; } 
    public List<ApprovalLineDto> ApprovalLines { get; set; }
}

public class ApprovalLineDto
{
    public string UserId { get; set; }
}