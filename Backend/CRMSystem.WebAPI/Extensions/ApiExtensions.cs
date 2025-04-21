using System.Text;
using CRMSystem.WebAPI.Auth;
using CRMSystem.WebAPI.Core;
using CRMSystem.WebAPI.Email;
using CRMSystem.WebAPI.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace CRMSystem.WebAPI.Extensions
{
    public static class ApiExtensions
    {
        public static void AddDbConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SchoolDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString(nameof(SchoolDbContext))));
        }

        public static void AddDbInitialization(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;
            var schoolContext = services.GetRequiredService<SchoolDbContext>();
            DbInitializer.Initialize(schoolContext);
        }

        public static void AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["jwt"];
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();
        }

        public static void AddAuthorizationPolicy(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPolicy(AuthorizationPolicies.AdminOnly, policy =>
                    policy.RequireRole(((int)Roles.Admin).ToString()))
                .AddPolicy(AuthorizationPolicies.UserOnly, policy =>
                    policy.RequireRole(((int)Roles.User).ToString()))
                .AddPolicy(AuthorizationPolicies.UserOrAdmin, policy =>
                    policy.RequireRole(((int)Roles.User).ToString(), ((int)Roles.Admin).ToString()));
        }

        public static void AddCookiePolicy(this IApplicationBuilder app)
        {
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            });
        }
        
        public static void AddSmtpConfiguration(this IServiceCollection services, ConfigurationManager configuration)
        {
            configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("smtpcredentials.json", optional: false, reloadOnChange: true);
            
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.Configure<EmailCredentials>(configuration.GetSection("EmailCredentials"));
        }
        
        public static void AddSerilogLogging(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}