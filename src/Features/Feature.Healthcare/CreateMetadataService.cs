using eXtensionSharp;
using Feature.Domain.Base;
using FluentValidation;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.Extensions.Logging;
using Riok.Mapperly.Abstractions;

namespace Feature.Healthcare;

public class MetadataDto
{
    public string Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int HeartRate { get; set; }
    public int Bpm { get; set; }
}

public interface ICreateMetadataService : IServiceImpl<MetadataDto, Results<string>>
{
    
}

public class CreateMetadataService : ServiceBase<CreateMetadataService, HealthcareDbContext,  MetadataDto, Results<string>>, ICreateMetadataService
{
    public CreateMetadataService(ILogger<CreateMetadataService> logger, ISessionContext sessionContext, HealthcareDbContext dbContext) : base(logger, sessionContext, dbContext)
    {
    }

    public override async Task<Results<string>> HandleAsync(MetadataDto request, CancellationToken cancellationToken)
    {
        var newItem = request.DtoToEntity();
        if (newItem.Id.xIsNotEmpty()) return await Results<string>.FailAsync("Invalid metadata");
        newItem.Id = Guid.CreateVersion7().ToString();
        await this.DbContext.Metadatas.AddAsync(newItem, cancellationToken);
        await this.DbContext.SaveChangesAsync(cancellationToken);

        return await Results<string>.SuccessAsync(newItem.Id);
    }
}

[Mapper]
public static partial class MetadataMapping
{
    public static partial Metadata DtoToEntity(this MetadataDto dto);
}