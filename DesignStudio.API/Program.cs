using Autofac;
using Autofac.Extensions.DependencyInjection;
using DesignStudio.API;
using DesignStudio.API.DI;

var builder = WebApplication.CreateBuilder(args);

// Use Autofac as the service provider
builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        // Register services via our DI class
        containerBuilder.AddAutofacServices(builder.Configuration);
    });

// Register framework services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
