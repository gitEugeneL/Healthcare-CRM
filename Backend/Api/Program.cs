using Api;
using Application;
using Infrastructure;
using Scalar.AspNetCore;
using ConfigureServices = Api.ConfigureServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    DevInitializer.Initialize(app.Services);

    app.MapOpenApi();
    app.MapScalarApiReference("/scalar");
}

ConfigureServices.MapEndpoints(app);

app.Run();
