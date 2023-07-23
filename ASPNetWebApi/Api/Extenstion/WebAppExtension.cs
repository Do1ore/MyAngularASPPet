using Microsoft.Extensions.FileProviders;

namespace MySuperApi.Extenstion;

public static class WebAppExtension
{
    public static void AddAndConfigureStaticFiles(this WebApplication app)
    {
        app.UseStaticFiles(
            new StaticFileOptions
            {
                FileProvider =
                    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/resources")),
                RequestPath = new PathString("/wwwroot/resources")
            });
    }

    public static void UseCustomSwaggerUI(this WebApplication app)
    {
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint(
                "/swagger/v1/swagger.json",
                "MessageApi V1");
            c.RoutePrefix = string.Empty;
        });
    }
}