using Microsoft.Extensions.FileProviders;
using MySuperApi.Areas;
using MySuperApi.Extenstion;
using MySuperApi.HubConfig;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.ConfigureMongo(builder.Configuration);

builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen();

builder.Services.AddFormsOptionsConfiguration();

builder.Services.AddCorsConfiguration();

builder.Services.AddCustomServices();
builder.Services.AddCustomRepositories();


builder.Services.ConfigureAuthentication(builder);

builder.Services.AddAndConfigureMediatR();

var app = builder.Build();

app.UseCors("default");
app.AddAndConfigureStaticFiles();

//SignalR route
app.MapHub<UserHub>("hub/user");

app.UseStaticFiles();
app.UseHttpsRedirection();
app.MapControllers();
app.UseSwagger();
app.UseCustomSwaggerUI();

app.Run();