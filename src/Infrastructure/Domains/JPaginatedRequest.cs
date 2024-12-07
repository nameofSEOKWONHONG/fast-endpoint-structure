﻿namespace Infrastructure.Domains;

public interface IPaginatedRequestBase
{
    int PageNo { get; set; }
    int PageSize { get; set; }    
}

public class JPaginatedRequest<T>
{
    public T Data { get; set; }
    public int PageNo { get; set; }
    public int PageSize { get; set; }
}