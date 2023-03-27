using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using SenseTowerEventAPI.Features.Event;
using SenseTowerEventAPI.Interfaces;
using SenseTowerEventAPI.Models.Context;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SenseTowerEventAPI.Models;
using SenseTowerEventAPI.Repository.EventRepository;
using SenseTowerEventAPI.Repository.TicketRepository;
using MediatR;
using Polly;
using SenseTowerEventAPI.Extensions.Behaviors;
using SenseTowerEventAPI.Extensions.Middleware;
using SenseTowerEventAPI.Extensions.Services;
#pragma warning disable CS0618

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddHttpClient<ITicketRepository, TicketRepository>( client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["ServiceEndpoints:PaymentServiceURL"] ?? string.Empty);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {builder.Configuration["ServiceEndpoints:TokenAuthorization"]}");
    })
    .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2)))
    .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(10)));

// Add services to the container.
builder.Services.AddControllers().AddFluentValidation(f => f.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddValidatorsFromAssemblyContaining<EventValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "¬ведите верный токен",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
        }
    });

    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Sense Tower Event API",
        Description = "API дл€ управлени€ меропри€ти€ми в Sense Tower"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    swagger.IncludeXmlComments(xmlPath);
});

DBContextMapper.InitRegisterMap();
builder.Services.Configure<EventContext>(builder.Configuration.GetSection("EventsDatabaseSettings"));

builder.Services.AddValidatorsFromAssemblyContaining<EventValidatorBehavior>();
builder.Services.AddScoped<IRabbitMQProducer, RabbitMQProducer>();
builder.Services.AddScoped<IEventValidatorBehavior, EventValidatorBehavior>();
builder.Services.AddSingleton<IEventSingleton, EventSingleton>();
builder.Services.AddSingleton<IEventValidatorRepository, EventValidatorRepository>();
builder.Services.AddSingleton<ITicketRepository, TicketRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

app.UseMiddleware<RequestResponseLoggingMiddleware>();

app.UseCors(b =>
{
    b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});

IdentityModelEventSource.ShowPII = true;
app.UseSwagger();
app.UseSwaggerUI();

app.UseHsts();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();