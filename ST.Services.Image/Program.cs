using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Polly;
using ST.Services.Image;
using ST.Services.Image.Interfaces;
using ST.Services.Image.Middleware;
using ST.Services.Image.Models;
using ST.Services.Image.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<RabbitMQConsumer>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = Environment.GetEnvironmentVariable("IdentityServer4Settings__Authority");
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters.ValidAudiences = new List<string?> { Environment.GetEnvironmentVariable("IdentityServer4Settings__Audience") };
});

builder.Services.AddAuthorization();

builder.Services.AddHttpClient<IImageServiceManager, ImageServiceManager>(client =>
{
    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ServiceEndpoints__EventService__URL") ?? string.Empty);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Environment.GetEnvironmentVariable("ServiceEndpoints__TokenAuthorization")}");
})
    .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2)))
    .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(10)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddSingleton<IImageSingleton, ImageSingleton>();
builder.Services.AddTransient<IImageServiceManager, ImageServiceManager>();
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