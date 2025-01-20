namespace Feature.Domain.Tour.Results;

public class TourResult
{
    public int Id { get; set; }
    public string Nation { get; set; }
    public string Region { get; set; }
    public string City { get; set; }
    public DateTime Date { get; set; }
    public int TourId { get; set; }
}
