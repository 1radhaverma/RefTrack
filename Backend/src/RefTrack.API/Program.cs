using System.Text;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RefTrack.API.BackgroundServices;
using RefTrack.API.Hubs;
using RefTrack.API.Middleware;
using RefTrack.API.Services;
using RefTrack.Application.Common.Behaviours;
using RefTrack.Application.Features.Companies.Commands;
using RefTrack.Application.Interface;
using RefTrack.Infrastructure.Persistence;
using RefTrack.Infrastructure.Rerpositories;
using RefTrack.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDBContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Supabase"),
        n => n.EnableRetryOnFailure(3)));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateCompanyCommand).Assembly));

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

builder.Services.AddValidatorsFromAssembly(typeof(CreateCompanyCommand).Assembly);

// Repositories
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IJobRoleRepository, JobRoleRepository>();
builder.Services.AddScoped<IReferrerRepository, ReferrerRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();

// Services
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddSingleton<INotificationService, SignalRNotificationService>();

// Background service
builder.Services.AddHostedService<ReminderService>();

builder.Services.AddSignalR();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
        opt.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                var token = ctx.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(token)
                    && ctx.HttpContext.Request.Path.StartsWithSegments("/hubs"))
                    ctx.Token = token;
                return Task.CompletedTask;
            }
        };
    });

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
    { Type = SecuritySchemeType.Http, Scheme = "bearer", Description = "Enter JWT" });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {{
        new OpenApiSecurityScheme { Reference = new OpenApiReference
            { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
        Array.Empty<string>()
    }});
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("Angular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ReminderHub>("/hubs/reminders");

app.Run();