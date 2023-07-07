using AspNetCoreRateLimit;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Microsoft.OpenApi.Models;

namespace HostME.Core
{
    public static class ServiceExtensions
    {
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtsettings = configuration.GetSection("Jwt");
            var key = Environment.GetEnvironmentVariable("KEY"); // This is the key I set in my global env on my local machine

            services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(op =>
            {
                op.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtsettings.GetSection("Issuer").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });
        }

        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        Log.Error($"Something went wrong. Err: {contextFeature.Error}");

                        await context.Response.WriteAsync(new ErrorConfig
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal server error, please try again later"
                        }.ToString());
                    }
                });
            });
        }

        public static void ConfigureHttpCacheHeaders(this IServiceCollection services)
        {

            services.AddResponseCaching();
            services.AddHttpCacheHeaders(expOpt =>
            {
                expOpt.MaxAge = 65;
                expOpt.CacheLocation = CacheLocation.Private;
            },

            (validationOpt) =>
            {
                validationOpt.MustRevalidate = true;
            }

            );
        }

        public static void ConfigureRateLimitting(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule> { 
                
                // So what this means is that to all the endpoints
                // All endpoints are limited to one(1) call every 10seconds
                new RateLimitRule {
                    Endpoint = "*",
                    Limit = 1,
                    Period = "10s"

                }
            };

            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = rateLimitRules;
            });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(op =>
            {
                op.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Jwt Authorization header using Bearer scheme. Enter 'Bearer' [space] and then your token in the text input Below 
                Example: 'Bearer 123thuwi'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                op.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "Oauth2",
                    Name = " Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });
            });
        }

    }
}
