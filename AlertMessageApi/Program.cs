using AlertMessageApi.Models;
using AlertMessageApi.Services;
using AlertMessageApi.Controllers;
using MongoDB;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

 using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<AlertMessageDbSettings>(
    builder.Configuration.GetSection("AlertMessageDbSettings"));
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<AlertMessageDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});
builder.Services.AddSingleton<AlertService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Print connection status when application starts.
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var alertService = serviceProvider.GetRequiredService<AlertService>();
    alertService.PrintConnectionStatus();
}

app.Run();
