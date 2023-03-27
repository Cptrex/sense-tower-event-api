using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Polly;
using ST.Services.Image.Extensions.Middleware;
using ST.Services.Image.Extensions.Services;
using ST.Services.Image.Interfaces;
using ST.Services.Image.Models;
using ST.Services.Image.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<RabbitMQConsumerService>();

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

builder.Services.AddHttpClient<IImageRepository, ImageRepository>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceEndpoints:EventServiceURL"] ?? string.Empty);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {builder.Configuration["ServiceEndpoints:TokenAuthorization"]}");
})
    .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2)))
    .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(10)));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddSingleton<IImageSingleton, ImageSingleton>();
builder.Services.AddTransient<IImageRepository, ImageRepository>();

var app = builder.Build();

app.UseMiddleware<RequestResponseLoggingMiddleware>();

app.UseCors(b =>
{
    b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});

IdentityModelEventSource.ShowPII = true;

app.UseHsts();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();