using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MySuperApi.Api.Areas;
using MySuperApi.Api.Extenstion;
using MySuperApi.Api.HubConfig;
using MySuperApi.Domain;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnectionString") ??
                       throw new InvalidOperationException("Connection string 'MySuperConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCustomServices();
builder.Services.ConfigureMongo(builder.Configuration);

builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen();

builder.Services.AddFormsOptionsConfiguration();
builder.Services.AddCorsConfiguration();


builder.Services.AddAutoMapper(typeof(AppMappingProfile));

builder.Services.ConfigureAuthentication(builder);

var app = builder.Build();

app.UseCors("default");
app.UseStaticFiles(
    new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
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