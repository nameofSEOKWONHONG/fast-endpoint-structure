namespace Feature.Domain.Base;

public interface IPaginatedRequestBase
{
    int PageNo { get; set; }
    int PageSize { get; set; }    
}

public class PaginatedRequest<T>: IPaginatedRequestBase
{
    public T Data { get; set; }
    public int PageNo { get; set; }
    public int PageSize { get; set; }
}