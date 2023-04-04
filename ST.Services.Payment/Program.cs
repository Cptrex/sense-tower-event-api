using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using ST.Services.Payment.Interfaces;
using ST.Services.Payment.Middleware;
using ST.Services.Payment.Models;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddSingleton<IPaymentsSingleton, PaymentsSingleton>();

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