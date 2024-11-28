using Microsoft.EntityFrameworkCore;

namespace MovieSharpApi.Infrastructure.EF;

public interface IEntityBuilderBase
{
    void Build(ModelBuilder builder);
}