using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace DietiEstate.WebApi.Configs;

/// <summary>
/// A transformer that adds a JWT security scheme to the OpenAPI document.
/// It appends the "Bearer" security scheme to the `SecuritySchemes` collection and defines
/// a corresponding security requirement in the API document.
/// </summary>
/// <remarks>
/// The added JWT security scheme allows API consumers to include a JSON Web Token (JWT) in the
/// Authorization header of HTTP requests to authenticate.
/// </remarks>
public class JwtSecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    /// <summary>
    /// Transforms the OpenAPI document by adding a JWT security scheme and its associated
    /// security requirement to the document.
    /// </summary>
    /// <param name="document">
    /// The OpenAPI document to modify by appending security schemes and requirements.
    /// </param>
    /// <param name="context">
    /// The context in which the transformation is being applied.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe cancellation requests.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation of transforming the OpenAPI document.
    /// </returns>
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