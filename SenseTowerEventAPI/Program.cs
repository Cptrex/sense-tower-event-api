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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServer4Settings:Authority"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters.ValidAudiences = new List<string>() { builder.Configuration["IdentityServer4Settings:Audience"] };
    });

builder.Services.AddAuthorization();

// Add services to the container.
#pragma warning disable CS0618
builder.Services.AddControllers().AddFluentValidation(f => f.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
#pragma warning restore CS0618

builder.Services.AddValidatorsFromAssemblyContaining<EventValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "¬ведите верный токен",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
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
builder.Services.Configure<EventContext>(builder.Configuration.GetSection("EventsDatabaseSettings"));

builder.Services.AddValidatorsFromAssemblyContaining<EventValidatorBehavior>();
builder.Services.AddScoped<IEventValidatorBehavior, EventValidatorBehavior>();
builder.Services.AddSingleton<IEventSingleton, EventSingleton>();
builder.Services.AddSingleton<IEventValidatorRepository, EventValidatorRepository>();
builder.Services.AddSingleton<ITicketRepository, TicketRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();


app.UseCors(b =>
{
    b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});

IdentityModelEventSource.ShowPII = true;
app.UseSwagger();
app.UseSwaggerUI();

app.UseHsts();
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


