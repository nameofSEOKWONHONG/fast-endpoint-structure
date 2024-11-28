using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace MovieSharpApi.OpenApi;

public class OpenApiSecuritySchemeTransformer
    : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        document.Info.Title = Constants.OpenApi.Title;
        document.Info.Description = Constants.OpenApi.Description;
        document.Info.Contact = new OpenApiContact
        {
            Name = Constants.OpenApi.Name,
            Email = Constants.OpenApi.Email,
            Url = new Uri(Constants.OpenApi.Website)
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
                            Id = Constants.OpenApi.SecuritySchemeBearer,
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
                { Constants.OpenApi.SecuritySchemeBearer, securitySchema }
            }
        };
        return Task.CompletedTask;
    }
}