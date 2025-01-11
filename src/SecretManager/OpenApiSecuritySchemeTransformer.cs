using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace SecretManager;

public class OpenApiSecuritySchemeTransformer
    : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        document.Info.Title = Constants.Title;
        document.Info.Description = Constants.Description;
        document.Info.Contact = new OpenApiContact
        {
            Name = Constants.Name,
            Email = Constants.Email,
            Url = new Uri(Constants.Website)
        };

        var securitySchema =
            new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme."
            };

        var securityRequirement =
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = Constants.SecuritySchemeBearer,
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    []
                }
            };
        
        document.SecurityRequirements.Add(securityRequirement);
        document.Components = new OpenApiComponents()
        {
            SecuritySchemes = new Dictionary<string, OpenApiSecurityScheme>()
            {
                { Constants.SecuritySchemeBearer, securitySchema }
            }
        };
        return Task.CompletedTask;
    }
}