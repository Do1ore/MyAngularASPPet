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

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v3", new OpenApiInfo { Title = "My API", Version = "v3" });
});

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v3/swagger.json", "My SuserAPI V3");
        c.RoutePrefix = string.Empty;
    });

}
app.UseCors("default");
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resourses")),
    RequestPath = new PathString("/Resourses")
});

//SignalR route
app.MapHub<UserHub>("hub/user");

app.UseStaticFiles();
app.UseHttpsRedirection();
app.MapControllers();
app.MapSwagger();

app.Run();
