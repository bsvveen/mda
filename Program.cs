using MDA.Infrastructure;
using MDA.Primitive.Database;
using MediatR;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o => { o.CustomSchemaIds(x => x.FullName); });
builder.Services.AddTransient<ISql, Sqlite>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Model")),
    RequestPath = "/Primitive",
    EnableDirectoryBrowsing = true
});

app.UseAuthorization();

app.MapControllers();

app.Run();
