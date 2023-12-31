using AspNetCoreRateLimit;
using HostME.API.Config;
using HostME.Core;
using HostME.Core.Services;
using HostME.Core.UnitOfWork;
using HostME.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
builder.Services.AddDbContext<HostMeContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"),
    b => b.MigrationsAssembly("HostME.API")));

// 4. Configuring Identity core
builder.Services.AddAuthentication();
builder.Services.AddIdentity<ApiUser, ApiRoles>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<HostMeContext>().AddDefaultTokenProviders();

// 5. Unit of Work Config
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

// 6. Adding our AutoMapper
builder.Services.AddAutoMapper(typeof(MapperConfig));

// 8. Adding our authmanager
builder.Services.AddScoped<IAuthManager, AuthManager>();

// 9. Configuring JWT
builder.Services.ConfigureJWT(builder.Configuration);

// 10. Adding Caching Abilities
builder.Services.ConfigureHttpCacheHeaders();
builder.Services.AddResponseCaching();

// 11. Adding Throttling abilities
// This is to keep track of which Client has made the request.
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitting();
builder.Services.AddHttpContextAccessor();

// 12. Adding swagger config for the sake of Auth JWT
builder.Services.ConfigureSwagger();


builder.Services.AddControllers(config =>
{
    // This is under step 10
    config.CacheProfiles.Add("120SecondsDuration", new CacheProfile
    {
        Duration = 120
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// App config
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

// 7. Configuring our global exception handler
app.ConfigureExceptionHandler();

// 1. Registering CORs Policy
app.UseCors(origins);

// 2. Registering our Logging service
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

// 10. Setting up caching
app.UseResponseCaching();
app.UseHttpCacheHeaders();

// 11. Configuring throttling
app.UseIpRateLimiting();

// 3. registering authentication which will help with our JWT config.
app.UseAuthentication();
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