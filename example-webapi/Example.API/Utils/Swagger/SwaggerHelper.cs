using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Example.API.Utils.Swagger;

public static class SwaggerHelper
{
    private const string DocumentName = "v1";
    private const string ApiTitle     = "Example API";

    // ====== SwaggerGen ======

    public static void ConfigureSwaggerGen(SwaggerGenOptions options)
    {
        options.SwaggerDoc(DocumentName, new OpenApiInfo
        {
            Title   = ApiTitle,
            Version = DocumentName
        });

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Bearer {access token}",
            Name        = "Authorization",
            In          = ParameterLocation.Header,
            Type        = SecuritySchemeType.ApiKey,
            Scheme      = "Bearer"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id   = "Bearer"
                    }
                },
                new List<string>()
            }
        });
    }

    // ====== Swagger Middleware ======

    public static void ConfigureSwagger(SwaggerOptions _) { }

    // ====== SwaggerUI ======

    public static void ConfigureSwaggerUI(SwaggerUIOptions options)
    {
        options.SwaggerEndpoint($"./swagger/{DocumentName}/swagger.json", ApiTitle);
        options.RoutePrefix = string.Empty;
        options.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
    }
}
