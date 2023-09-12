using System.Reflection;
using System.Text;
using Application.Features.Chat.CreateChat;
using Infrastructure.Abstraction;
using Infrastructure.Abstraction.Repositories;
using Infrastructure.Abstraction.Services;
using Infrastructure.Abstraction.Services.Token;
using Infrastructure.Abstraction.Services.User;
using Infrastructure.Implementation.Repositories;
using Infrastructure.Implementation.Services;
using Infrastructure.Implementation.Services.PathLogic;
using Infrastructure.Implementation.Services.ProfileImageService;
using Infrastructure.Implementation.Services.Token;
using Infrastructure.Implementation.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace MySuperApi.Extenstion;

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
        services.AddScoped<IHttpUserDataAccessorService, HttpUserDataAccessorService>();
        services.AddScoped<IPathMasterService, PathMasterService>();

        services.AddTransient<IProfileImageService, ProfileImageService>();
        services.AddTransient<IPasswordHashService, PasswordHashService>();

        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IImageService, ImageService>();
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

    public static void AddCustomRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMongoChatRepository, MongoChatRepository>();
        services.AddScoped<IMongoChatMessageRepository, MongoChatMessageRepository>();
    }

    public static void AddAndConfigureMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg
            .RegisterServicesFromAssembly(
                Assembly.GetAssembly(typeof(CreateChatRequest)) ??
                throw new InvalidOperationException()));
    }
}