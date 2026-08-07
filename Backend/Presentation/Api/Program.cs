using Api;
using Api.ApiConfiguration;
using Application;
using Persistence;
using Scalar.AspNetCore;
using ConfigureServices = Api.ConfigureServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    DevInitializer.Initialize(app.Services);

    app.MapOpenApi();
    
    app.MapScalarApiReference("/scalar", options =>
        options
            .AddPreferredSecuritySchemes("BearerAuth")
            .AddHttpAuthentication("BearerAuth", auth =>
            {
                auth.Token = ""; 
            })
        );
}

app.Run();
