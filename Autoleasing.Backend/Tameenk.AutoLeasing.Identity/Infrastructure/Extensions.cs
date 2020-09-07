
using Tameenk.AutoLeasing.Identity.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Tameenk.AutoLeasing.Identity
{
    public static class Extensions
    {
        private const string IdentitySectionName = "adminIdentity";

        public static void AddAdminIdentityContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AdminConnection");
            services.AddDbContext<AdminContext>(o => o.UseSqlServer(connectionString));
            services.AddAdminDependencies();
            services.AddAdminIdentityConfiguration();
         

        }

        private static void AddAdminDependencies(this IServiceCollection services)
        {
            services.AddTransient<IAdminService, AdminService>();
           // services.AddTransient<IAdminRoleService, AdminRoleService>();

        }

        public static void AddAdminIdentityConfiguration(this IServiceCollection services)
        {

            services.AddIdentity<ApplicationUser, ApplicationRole>(
                options =>
                {
                    options.Password = new PasswordOptions
                    {
                        RequiredLength = 6,
                        RequireUppercase = false,
                        RequireNonAlphanumeric = false,
                        RequireDigit = false,
                        RequireLowercase = false
                    };
                    options.User.AllowedUserNameCharacters = string.Empty;
                }).AddUserValidator<UsernameValidator<ApplicationUser>>()
                .AddEntityFrameworkStores<AdminContext>()
                .AddDefaultTokenProviders();

            IdentitySettings identitySettings;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                identitySettings = configuration.GetOptions<IdentitySettings>(IdentitySectionName);
                services.Configure<IdentitySettings>(configuration.GetSection(IdentitySectionName));
            }

            // JWT
            var key = Encoding.UTF8.GetBytes(identitySettings.JWT_Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = identitySettings.RequireHttpsMetadata;
                x.SaveToken = identitySettings.SaveToken;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = identitySettings.ValidateIssuer,
                    ValidateAudience = identitySettings.ValidateAudience,
                    ClockSkew = TimeSpan.Zero
                };
            });

        }

        public static void AddConnectionToDbContext(this DbContextOptionsBuilder optionsBuilder,string ConnectionStringName) 
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var basePath = AppContext.BaseDirectory;
            var builder = new ConfigurationBuilder()
                            .SetBasePath(basePath)
                            .AddJsonFile("appsettings.json")
                            .AddJsonFile($"appsettings.{environmentName}.json", true)
                          ;

            var config = builder.Build();

            var connection = config.GetConnectionString(ConnectionStringName);
            optionsBuilder.UseSqlServer(connection, options => options.EnableRetryOnFailure());
        }

       
    }
}
