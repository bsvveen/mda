using MDA.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o => { o.CustomSchemaIds(x => x.FullName); });
builder.Services.Configure<JsonOptions>(o => { o.SerializerOptions.Converters.Add(new JsonStringEnumConverter());});
builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(o => { o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());});

builder.Services.AddSingleton(provider => new ApplicationInstance());

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "FrontEnd")),
        RequestPath = "/FrontEnd"
    });
}

app.UseHttpsRedirection();

app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Model")),
    RequestPath = "/Model",
    EnableDirectoryBrowsing = true
});

app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();

app.Run();
