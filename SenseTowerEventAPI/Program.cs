using FluentValidation;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using SenseTowerEventAPI.Interfaces;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SenseTowerEventAPI.Models;
using MediatR;
using Polly;
using SenseTowerEventAPI.Features.Event.EventCreate;
using SenseTowerEventAPI.Middleware;
using SenseTowerEventAPI.Behaviors;
using SenseTowerEventAPI.RabbitMQ;
using SenseTowerEventAPI.MongoDB.Context;
using SenseTowerEventAPI.MongoDB;
using SenseTowerEventAPI.Features.Ticket;
using SenseTowerEventAPI.Features.Event;
using SenseTowerEventAPI.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<RabbitMQConsumer>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServer4Settings:Authority"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters.ValidAudiences = new List<string?> { builder.Configuration["IdentityServer4Settings:Audience"] };
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpClient<ITicketManager, TicketManager>("paymentService",client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["ServiceEndpoints:PaymentService:URL"] ?? string.Empty);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {builder.Configuration["ServiceEndpoints:TokenAuthorization"]}");
    })
    .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2)))
    .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(10)));

builder.Services.AddHttpClient<ITicketManager, TicketManager>("imageService", client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["ServiceEndpoints:ImageService:URL"] ?? string.Empty);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {builder.Configuration["ServiceEndpoints:TokenAuthorization"]}");
    })
    .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2)))
    .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(10)));

builder.Services.AddHttpClient<ITicketManager, TicketManager>("spaceService", client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["ServiceEndpoints:SpaceService:URL"] ?? string.Empty);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {builder.Configuration["ServiceEndpoints:TokenAuthorization"]}");
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