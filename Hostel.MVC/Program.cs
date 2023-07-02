
using Hostel.Data.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Database Config
builder.Services.AddDbContext<HostMeContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"), b => b.MigrationsAssembly("Hostel.Data")));


//Configuring Logging
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Setting up CORs policy
var origins = "_myOrgins";
builder.Services.AddCors(op =>
{
    op.AddPolicy(
        name: origins,
        policy => policy.WithOrigins("http://localhost:4000",
                                              "http://localhost:5173")
        );
});



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

// Registering our Logging service
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