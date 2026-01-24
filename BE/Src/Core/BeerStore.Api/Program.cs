using Api.Core.Logging;
using Api.Core.Middleware;
using Application.Core.Interface.ISettings;
using BeerStore.Infrastructure;
using Infrastructure.Core.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers();

// Add Swagger/OpenAPI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Beer Store API",
        Version = "v1",
        Description = "API for managing Beer Store - Authentication, Users, Roles, Permissions",
        Contact = new OpenApiContact
        {
            Name = "Beer Store Team",
            Email = "danhuy@gmail.com",
            Url = new Uri("https://example.com")
        }
    });

    // Config Swagger => JWT Bearer Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer eyJhbGci...'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
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

// Add Serilog
builder.Host.UseSharedSerilog(builder.Configuration);

// Add Infrastructure Services (DbContext, Repositories, UnitOfWork, JWT Service, Password Hasher)
builder.Services.AddAuthInfrastructure(builder.Configuration);
builder.Services.AddShopInfrastructure(builder.Configuration);

// Config JWT Settings (Load JWT => appsettings.json)
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
if (jwtSettings == null)
{
    throw new InvalidOperationException("JwtSettings not configured in appsettings.json");
}
builder.Services.AddSingleton<IJwtSettings>(jwtSettings);

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(BeerStore.Application.Interface.Services.IJwtService).Assembly
));

// Config JWT Authentication
var secretKey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
builder.Services
    .AddAuthentication(options =>
    {
        // Use JWT Bearer Default
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        // Config validate JWT token
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// AddAuthorization
builder.Services.AddAuthorization();

// Config CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Beer Store API v1");
        c.RoutePrefix = string.Empty;
    });
}

// Exception Middleware
app.UseMiddleware<ExceptionMiddleware>();

// Auto chuyển HTTP => HTTPS
app.UseHttpsRedirection();

// CORS Middleware
app.UseCors("AllowAll");

app.UseRouting();

// Authentication Middleware
app.UseAuthentication();

// Authorization Middleware
app.UseAuthorization();

// Map Controllers
app.MapControllers();

app.Run();