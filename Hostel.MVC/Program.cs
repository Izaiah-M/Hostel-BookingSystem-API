using HostME.API.Config;
using HostME.Core.UnitOfWork;
using HostME.Data.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// These services are added in an order that should be followed

// 1. Setting up CORs policy
var origins = "_myOrigins";
builder.Services.AddCors(op =>
{
    op.AddPolicy(
        name: origins,
        policy => policy.WithOrigins("http://localhost:4000",
                                              "http://localhost:5173")
        );
});

// 2. Configuring Logging
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// 3. Database Config
builder.Services.AddDbContext<HostMeContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"), b => b.MigrationsAssembly("Hostel.Data")));

// 4. Unit of Work Config
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

// 5. Adding our AutoMapper
builder.Services.AddAutoMapper(typeof(MapperConfig));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

// 1. Registering CORs Policy
app.UseCors(origins);

// 2. Registering our Logging service
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

try
{
    Log.Logger.Information("Server is successfully running");
    app.Run();

}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "Server failed to start");
    throw;
}