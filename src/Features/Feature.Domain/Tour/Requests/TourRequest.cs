using System.ComponentModel.DataAnnotations;

namespace Feature.Domain.Tour.Requests;

public class TourRequest
{
    [Required]
    public string Nation { get; set; }
    public string Region { get; set; }
    public string City { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    
    public int PageNo { get; set; }
    public int PageSize { get; set; }
}