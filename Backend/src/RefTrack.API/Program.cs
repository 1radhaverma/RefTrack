//using Scalar.AspNetCore;


//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddOpenApi();
//// Add services to the container.
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//    app.MapScalarApiReference(); 
//}

//app.UseHttpsRedirection();

//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast =  Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast");

//app.Run();

//record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
//{
//    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//}
// ── NuGet: Swashbuckle.AspNetCore needed for UseSwagger ──
// dotnet add package Swashbuckle.AspNetCore --version 6.5.0

using System;
using System.Text;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RefTrack.API.Middleware;
using RefTrack.Application.Common.Behaviours;
using RefTrack.Application.Features.Companies.Commands;
using RefTrack.Application.Interface;
using RefTrack.Infrastructure.BackgroundServices;
using RefTrack.Infrastructure.Hubs;
using RefTrack.Infrastructure.Persistence;
using RefTrack.Infrastructure.Rerpositories;
using RefTrack.Infrastructure.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. DATABASE
builder.Services.AddDbContext<AppDBContext>(opt =>
    opt.UseNpgsql(
        builder.Configuration.GetConnectionString("Supabase"),
        npgsql => npgsql.EnableRetryOnFailure(3)));

// 2. MEDIATR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(CreateCompanyCommand).Assembly));

// 3. PIPELINE BEHAVIOURS
builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

// 4. FLUENT VALIDATION
builder.Services.AddValidatorsFromAssembly(
    typeof(CreateCompanyCommand).Assembly);

// 5. REPOSITORIES — note the < on every AddScoped
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IJobRoleRepository, JobRoleRepository>();
builder.Services.AddScoped<IReferrerRepository, ReferrerRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();

// 6. SERVICES
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// 7. BACKGROUND SERVICE
builder.Services.AddHostedService<ReminderService>();

// 8. SIGNALR
builder.Services.AddSignalR();

// 9. JWT AUTH
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]!))
        };
        opt.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(token)
                    && path.StartsWithSegments("/hubs"))
                    context.Token = token;
                return Task.CompletedTask;
            }
        };
    });

// 10. CORS
builder.Services.AddCors(o =>
    o.AddPolicy("Angular", p =>
        p.WithOrigins("http://localhost:4200")
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Description = "Enter JWT token"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {{
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id   = "Bearer"
            }
        },
        Array.Empty<string>()
    }});
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapScalarApiReference(); // Scalar UI at /scalar
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("Angular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ReminderHub>("/hubs/reminders");

app.Run();