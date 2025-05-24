using System.IO;
using Microsoft.EntityFrameworkCore;
using DesignStudio.DAL.Data;
using DesignStudio.BLL.Interfaces;
using DesignStudio.BLL.Services;
using DesignStudio.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using DesignStudio.BLL.Mapping;
using DesignStudio.API.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Логування
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Конфігурація через appsettings.json
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(cfg =>
{
    // Профіль із BLL: мапінги DAL ↔ BLL DTO
    cfg.AddProfile<MappingProfile>();
    // Профіль із API: мапінги API DTO ↔ BLL DTO
    cfg.AddProfile<ApiMappingProfile>();
});

// Зчитуємо connection string з appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Реєструємо DbContext та IDbContext
builder.Services.AddDbContext<DesignStudioContext>(opts =>
    opts.UseSqlite(connectionString));
builder.Services.AddScoped<DesignStudio.DAL.Data.IDbContext>(sp => sp.GetRequiredService<DesignStudioContext>());

// Репозиторії та UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// BLL менеджери
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<IOrderManager, OrderManager>();
builder.Services.AddScoped<IPortfolioManager, PortfolioManager>();

// Фасадний сервіс
builder.Services.AddScoped<IDesignStudioService, DesignStudioService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Автоматичне застосування міграцій
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DesignStudioContext>();
    db.Database.Migrate();
}

// Middleware
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DesignStudio API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseAuthorization();

app.MapControllers();
app.MapGet("/ping", () => Results.Ok("pong"));

app.Run();