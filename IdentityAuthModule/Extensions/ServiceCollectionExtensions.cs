using IdentityAuthModule.Persistence;
using IdentityAuthModule.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IdentityAuthModule.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthModule(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthDbContext(config);
            services.ConfigureIdentityOptions();
            services.AddIdentityCoreServices();
            services.ConfigureJwtBearer(config);

            services.AddScoped<IAuthService, AuthService>();
            services.AddAuthorization();

            return services;
        }

        private static void AddAuthDbContext(this IServiceCollection services, IConfiguration config)
        {
            var connection = config.GetConnectionString("AuthConnection");
            if (string.IsNullOrWhiteSpace(connection))
                throw new InvalidOperationException("Connection string 'AuthConnection' is missing.");

            services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(connection));
        }

        private static void ConfigureIdentityOptions(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 1;
            });
        }

        private static void AddIdentityCoreServices(this IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>(options => { /* opcional */ })
                    .AddEntityFrameworkStores<AuthDbContext>()
                    .AddDefaultTokenProviders();
        }

        private static void ConfigureJwtBearer(this IServiceCollection services, IConfiguration config)
        {
            var secret = config["Jwt:Secret"];
            var issuer = config["Jwt:Issuer"];
            var audience = config["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(secret) || string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
                throw new InvalidOperationException("JWT configuration is missing or incomplete.");

            var key = Encoding.UTF8.GetBytes(secret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.Response.StatusCode = 401;
                        context.Response.Headers.Append("WWW-Authenticate", "Bearer");
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
