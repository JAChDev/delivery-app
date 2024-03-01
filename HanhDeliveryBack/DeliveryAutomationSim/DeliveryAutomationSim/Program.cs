using DeliveryAutomationSim.Services;
using DeliveryAutomationSim.Services.Interfaces;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using DeliveryAutomationSim.Services.Background;
using DeliveryAutomationSim.Controllers;
using DeliveryAutomationSim.Services.Hubs;

var builder = WebApplication.CreateBuilder(args);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

// Create an instance of GraphService class and execute build process in order to use it
builder.Services.AddSingleton<IGraphService, GraphService>();
builder.Services.AddSingleton<IAutomationServices, AutomationServices>();
builder.Services.AddHostedService<BackgroundExecutionService>();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(end =>
{
    end.MapHub<NotificationHub>("/notificationHub");
});

app.MapControllers();

app.Run();

