namespace Feature.Domain.Product;

public class CreatePlanRequest
{
    public long Id { get; set; }
    public string Title { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string Step { get; set; } 
    public List<ApprovalLineRequest> ApprovalLines { get; set; }
}

public class ApprovalLineRequest
{
    public string UserId { get; set; }
}