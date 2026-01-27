using Adapters;
using Infraestructure;
using Persistence;
using System.Reflection;
using Asp.Versioning.Conventions;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Api.Helpers;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;
using MediatR;
using Api.Behaviors;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Agregar services por defecto.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Backed", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor ingrese el JWT con Bearer en el campo, tenga en cuenta las audiencias.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
   {
     new OpenApiSecurityScheme
     {
       Reference = new OpenApiReference
       {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
       }
      },
      Array.Empty<string>()
    }
  });
});

// Agregar versionamiento de API/Controller
builder.Services.AddApiVersioning(
    options =>
    {
        options.ReportApiVersions = true;
    })
    .AddMvc(
    options =>
    {
        // Aplica automáticamente una versión de API basada en el Namespace
        options.Conventions.Add(new VersionByNamespaceConvention());
    });

// Agregar politica CORS, para multiples dominios
const string corsPolicy = "CorsPolicy";
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicy,
                      policy =>
                      {
                          policy.WithOrigins(allowedOrigins);
                          policy.AllowAnyMethod();
                          policy.AllowAnyHeader();
                      });
});

// Agregar injección de dependencias
builder.Services.AddAdapterServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);

// Agregar injección de dependencias para FluentValidation
const string ApplicationAssemblyName = "Application";
var applicationAssembly = Assembly.Load(ApplicationAssemblyName);
builder.Services.AddMediatR(x =>
{
    x.RegisterServicesFromAssemblies(applicationAssembly);
});

// Registrar todos los validadores de FluentValidation en el ensamblado Application
builder.Services.AddValidatorsFromAssembly(Assembly.Load(ApplicationAssemblyName));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Agregar servicios para solicitar JWT desde Azure AD
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// Agregar solicitud de configuración Bearer JWT
builder.Services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true
    };
    if (builder.Configuration.GetSection("Audiences").Get<string[]>() != null && builder.Configuration.GetSection("Audiences").Get<string[]>().Length > 0)
    {
        options.TokenValidationParameters.ValidateAudience = true;
        options.TokenValidationParameters.ValidAudiences = builder.Configuration.GetSection("Audiences").Get<string[]>();
    }
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            SimulatedUserHelper.UseSimulatedUser(context, builder.Configuration);
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication Failed: {context.Exception}");
            return Task.CompletedTask;
        }
    };
});

// Agregar configuraciones requeridas por los nuget
//builder.Services.AddManejoExcepcionesDI(builder.Configuration);

var app = builder.Build();

// Solo para ambientes de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// Agregar solicitud de autenticación y autorización en los Controller
app.UseAuthentication();
app.UseAuthorization();

// Implementación de Middleware personalizados
//app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(corsPolicy);

app.MapControllers();

app.Run();