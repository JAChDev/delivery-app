using DeliveryAutomationSim.Services;
using DeliveryAutomationSim.Services.Interfaces;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

builder.Services.AddSingleton<IGraphService, GraphService>();

// Create an instance of GraphService class and execute build process in order to use it
builder.Services.AddSingleton<GraphService>();

var graphService = builder.Services.BuildServiceProvider().GetRequiredService<GraphService>();
graphService.LoadAndBuildGraph().GetAwaiter().GetResult();
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

app.Run();
