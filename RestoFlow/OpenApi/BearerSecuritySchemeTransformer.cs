using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace RestoFlow.OpenApi
{
    internal sealed class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
    {
        public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            var schemes = await authenticationSchemeProvider.GetAllSchemesAsync();
            if (!schemes.Any(s => s.Name == JwtBearerDefaults.AuthenticationScheme))
            {
                return;
            }

            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
            document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Paste the JWT returned from /api/Auth/login (no 'Bearer ' prefix needed)."
            };

            var schemeReference = new OpenApiSecuritySchemeReference("Bearer", document, null);
            var requirement = new OpenApiSecurityRequirement
            {
                [schemeReference] = new List<string>()
            };

            foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations.Values))
            {
                operation.Security ??= new List<OpenApiSecurityRequirement>();
                operation.Security.Add(requirement);
            }
        }
    }
}
