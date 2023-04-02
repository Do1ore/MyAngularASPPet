using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MySuperApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("MySuperConnection") ?? throw new InvalidOperationException("Connection string 'MySuperConnection' not found.");
builder.Services.AddDbContext<MyPetContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(setupAction =>
{
    setupAction.AddPolicy("default",
        builder =>
        {
            builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
        });
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v3", new OpenApiInfo { Title = "My API", Version = "v3" });
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
app.UseHttpsRedirection();

app.MapControllers();
app.MapSwagger();
app.Run();
