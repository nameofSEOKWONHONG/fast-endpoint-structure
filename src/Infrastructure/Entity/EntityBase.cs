namespace Infrastructure.Entity;

public interface IEntityBase
{
    string CreatedBy { get; set; }
    DateTime CreatedOn { get; set; }
    string ModifiedBy { get; set; }
    DateTime? ModifiedOn { get; set; }
}

public class EntityBase : IEntityBase
{
    /// <summary>
    /// 생성자
    /// </summary>
    public string CreatedBy { get; set; }
    /// <summary>
    /// 생성일
    /// </summary>
    public DateTime CreatedOn { get; set; }
    
    /// <summary>
    /// 수정자
    /// </summary>
    public string? ModifiedBy { get; set; }
    
    /// <summary>
    /// 수정일
    /// </summary>
    public DateTime? ModifiedOn { get; set; }

    /// <summary>
    /// 활성화 여부 - 삭제 대용
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    protected EntityBase()
    {
    }

    protected EntityBase(string createdBy, DateTime createdOn)
    {
        CreatedBy = createdBy;
        CreatedOn = createdOn;
    }
}