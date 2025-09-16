using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace DietiEstate.WebApi.Configs;

public class JwtSecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Enter your JWT token in the text input below.\n\nExample: \"abcdef12345\""
        };

        var jwtSecurityRequirement = new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        };

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, OpenApiSecurityScheme>();
        
        document.Components.SecuritySchemes.Add("Bearer", jwtSecurityScheme);
        document.SecurityRequirements.Add(jwtSecurityRequirement);

        return Task.CompletedTask;
    }
}