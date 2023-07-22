using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using MySuperApi.Infrastructure.Repositories.Implementation;
using MySuperApi.Infrastructure.Repositories.Interfaces;
using MySuperApi.Infrastructure.Repositories.Services.PathLogic;
using MySuperApi.Infrastructure.Repositories.Services.ProfileImageService;
using MySuperApi.Infrastructure.Repositories.Services.UserService;

namespace MySuperApi.Api.Extenstion;

public static class ServicesExtenstion
{
    public static void ConfigureAuthentication(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value ??
                                  throw new InvalidOperationException("AppSettings -> Token not found"))),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
    }

    public static void AddFormsOptionsConfiguration(this IServiceCollection services)
    {
        services.Configure<FormOptions>(options =>
        {
            options.MemoryBufferThreshold = int.MaxValue;
            options.ValueLengthLimit = int.MaxValue;
            options.MultipartBodyLengthLimit = int.MaxValue;
        });
    }

    public static void AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(setupAction =>
        {
            setupAction.AddPolicy("default",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });
    }

    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPathMaster, PathMaster>();
        services.AddTransient<IProfileImageService, ProfileImageService>();
        services.AddScoped<IChatRepository, ChatRepository>();
    }

    public static void ConfigureMongo(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoConnection = configuration.GetSection("MongoSettings")["MongoConnection"] ??
                              throw new ArgumentNullException($"Connection for mongo not found");

        var databaseName = configuration.GetSection("MongoSettings")["DatabaseName"] ??
                           throw new ArgumentNullException($"Connection for mongo not found");
        services.AddSingleton<IMongoClient>(s => new MongoClient(mongoConnection));

        services.AddScoped<IMongoDatabase>(s =>
        {
            var client = s.GetRequiredService<IMongoClient>();
            return client.GetDatabase(databaseName);
        });
    }
}