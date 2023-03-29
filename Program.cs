using MDA.Infrastructure;
using Microsoft.AspNetCore.Authentication;
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

builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddSingleton(provider => new ApplicationInstance());

builder.Services.AddAuthentication(o => {
    o.DefaultScheme = "Application"; // CookieAuthenticationDefaults.AuthenticationScheme;
    o.DefaultSignInScheme = "External"; //CookieAuthenticationDefaults.AuthenticationScheme;
    o.DefaultAuthenticateScheme = "External";
})
.AddCookie("Application", options => {
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
})
.AddCookie("External")
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});

builder.Services.AddAuthorization();


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.Use(async (context, next) => {
    context.Response.Headers.Add("Content-Security-Policy", "script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; img-src * data:;");
    if (!context.User.Identity.IsAuthenticated && context.Request.Path != "/signin-google" && context.Request.Path != "/Account/Login" & context.Request.Path != "/Account/LoginCallBack")
    {
        await context.ChallengeAsync("External");
    }
    else
    {
        await next();
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseStaticFiles();

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "FrontEnd")),
        RequestPath = "/FrontEnd"
    });
}



//app.UseCors(policy => policy.SetIsOriginAllowed(origin => origin == "https://login.microsoftonline.com//"));

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Model")),
    RequestPath = "/Model"
});

app.MapRazorPages();
app.MapControllers();

app.Run();
