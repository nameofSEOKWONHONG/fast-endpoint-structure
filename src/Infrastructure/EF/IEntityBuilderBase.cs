using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF;

public interface IEntityBuilderBase
{
    void Build(ModelBuilder builder);
}