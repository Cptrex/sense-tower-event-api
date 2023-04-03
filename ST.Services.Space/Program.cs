using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Polly;
using ST.Services.Space;
using ST.Services.Space.Interfaces;
using ST.Services.Space.Middleware;
using ST.Services.Space.Models;
using ST.Services.Space.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<RabbitMQConsumer>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["IdentityServer4Settings:Authority"];
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters.ValidAudiences = new List<string?> { builder.Configuration["IdentityServer4Settings:Audience"] };
});

builder.Services.AddAuthorization();

builder.Services.AddHttpClient<ISpaceServiceManager, SpaceServiceManager>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["ServiceEndpoints:EventServiceURL"] ?? string.Empty);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {builder.Configuration["ServiceEndpoints:TokenAuthorization"]}");
    })
    .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2)))
    .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(10)));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddSingleton<ISpaceSingleton, SpaceSingleton>();
builder.Services.AddTransient<ISpaceServiceManager, SpaceServiceManager>();

builder.Services.AddSingleton<IRabbitMQConfigure, RabbitMQConfigure>();

var app = builder.Build();

app.UseMiddleware<RequestResponseLoggingMiddleware>();

app.UseCors(b =>
{
    b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});

IdentityModelEventSource.ShowPII = true;

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();