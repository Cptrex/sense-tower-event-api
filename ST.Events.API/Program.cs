using FluentValidation;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using ST.Events.API.Interfaces;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ST.Events.API.Models;
using MediatR;
using Polly;
using ST.Events.API.Features.Event.EventCreate;
using ST.Events.API.Middleware;
using ST.Events.API.Behaviors;
using ST.Events.API.RabbitMQ;
using ST.Events.API.MongoDB.Context;
using ST.Events.API.MongoDB;
using ST.Events.API.Features.Ticket;
using ST.Events.API.Features.Event;
using ST.Events.API.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<RabbitMQConsumer>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = Environment.GetEnvironmentVariable("IdentityServer4Settings__Authority");
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters.ValidAudiences = new List<string?> { Environment.GetEnvironmentVariable("IdentityServer4Settings__Audience") };
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpClient("paymentService",client =>
    {
        client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ServiceEndpoints__PaymentService__URL") ?? string.Empty);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Environment.GetEnvironmentVariable("ServiceEndpoints__TokenAuthorization")}");
    })
    .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2)))
    .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(10)));

builder.Services.AddHttpClient("imageService", client =>
    {
        client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ServiceEndpoints__ImageService__URL") ?? string.Empty);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Environment.GetEnvironmentVariable("ServiceEndpoints__TokenAuthorization")}");
    })
    .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2)))
    .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(10)));

builder.Services.AddHttpClient("spaceService", client =>
    {
        client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ServiceEndpoints__SpaceService__URL") ?? string.Empty);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Environment.GetEnvironmentVariable("ServiceEndpoints__TokenAuthorization")}");
    })
    .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2)))
    .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(10)));

builder.Services.AddScoped<StValidationFilter>();

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddValidatorsFromAssemblyContaining<EventCreateValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "������� ������ �����",
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
        Description = "API ��� ���������� ������������� � Sense Tower"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    swagger.IncludeXmlComments(xmlPath);
});

DBContextMapper.InitRegisterMap();

builder.Services.AddValidatorsFromAssemblyContaining<EventCreateValidatorBehavior>();
builder.Services.AddScoped<IRabbitMQProducer, RabbitMQProducer>();
builder.Services.AddScoped<IEventCreateValidatorBehavior, EventCreateValidatorBehavior>();
builder.Services.AddSingleton<IEventSingleton, EventSingleton>();
builder.Services.AddSingleton<IEventValidatorManager, EventValidatorManager>();
builder.Services.AddSingleton<ITicketManager, TicketManager>();
builder.Services.AddSingleton<IMongoDBCommunicator, MongoDBCommunicator>();
builder.Services.AddSingleton<IRabbitMQConfigure, RabbitMQConfigure>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

IdentityModelEventSource.ShowPII = true;
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();