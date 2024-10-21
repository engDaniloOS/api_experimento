using Microsoft.OpenApi.Models;

namespace WebApi.Configurations
{
    public static class SwaggerServiceConfig
    {
        //private const string SECURITY_ID = "Bearer";

        public static void Configure(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Api para teste de recursos",
                    Version = "v1"
                });

                //s.AddSecurityDefinition(SECURITY_ID, new OpenApiSecurityScheme
                //{
                //    In = ParameterLocation.Header,
                //    Description = "Enter bearer token",
                //    Name = "Authorization",
                //    BearerFormat = "Bearer {token}",
                //    Type = SecuritySchemeType.ApiKey
                //});

                //s.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //         new OpenApiSecurityScheme {Reference = new OpenApiReference { Id = SECURITY_ID, Type = ReferenceType.SecurityScheme } },
                //         new List<string>()
                //    }
                //});
            });
        }
    }
}