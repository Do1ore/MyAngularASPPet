using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MySuperApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MySuperApi.Services.UserService;
using MySuperApi.Services.PathLogic;
using MySuperApi.Areas;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using MySuperApi.Services.ProfileImageService;
using MySuperApi.HubConfig;
using MySuperApi.Repositories.Interfaces;
using MySuperApi.Repositories.Implementation;
using MySuperApi.Extensions;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnectionString") ?? throw new InvalidOperationException("Connection string 'MySuperConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPathMaster, PathMaster>();
builder.Services.AddTransient<IProfileImageService, ProfileImageService>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(setupAction =>
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
builder.Services.Configure<FormOptions>(options =>
{
    options.MemoryBufferThreshold = int.MaxValue;
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = int.MaxValue;
});

builder.Services.ConfigureSwagger();

builder.Services.AddAutoMapper(typeof(AppMappingProfile));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

app.UseCors("default");
app.UseStaticFiles(
    new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resourses")),
        RequestPath = new PathString("/Resourses")
    });

//SignalR route
app.MapHub<UserHub>("hub/user");

app.UseStaticFiles();
app.UseHttpsRedirection();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint(
        "/swagger/v1/swagger.json",
        "MessageApi V1");
    c.RoutePrefix = string.Empty;
});

app.Run();
