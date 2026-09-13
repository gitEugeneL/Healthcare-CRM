using Api;
using Api.ApiConfiguration;
using Api.Setups;
using Application;
using Cache;
using Mail;
using Persistence;
using Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddCacheServices(builder.Configuration);
builder.Services.AddSecurityServices(builder.Configuration);
builder.Services.AddMailServices();
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

app.UseRateLimiter();

app.MapEndpoints();

app.UeDevConfiguration();

app.Run();
