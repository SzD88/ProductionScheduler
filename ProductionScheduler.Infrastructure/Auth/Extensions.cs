using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ProductionScheduler.Infrastructure.DAL;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ProductionScheduler.Application.Security;

namespace ProductionScheduler.Infrastructure.Auth
{
    internal static class Extensions
    {
        private const string SectionName = "auth";
        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {


            //  services.Configure<AuthOptions>(configuration.GetRequiredSection(SectionName));

            var options = configuration.GetOptions<AuthOptions>(SectionName);

            // services.AddSingleton<IAuthenticator, Authenticator>();

            services
                .Configure<AuthOptions>(configuration.GetRequiredSection(SectionName))
            .AddSingleton<IAuthenticator, Authenticator>()
            .AddSingleton<ITokenStorage, HttpContextTokenStorage>()
           .AddAuthentication(x =>
           {
               x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
           }).AddJwtBearer(x =>
           {
               x.Audience = options.Audience;
               x.IncludeErrorDetails = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidIssuer = options.Issuer,
                   ClockSkew = TimeSpan.Zero, //dodatkowy bufor czasu zycia tokena #refactor
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                   .GetBytes(options.SigningKey)),
               };
           }
            );
            services.AddAuthorization(authorization =>

            authorization.AddPolicy("is-admin", policy =>
            {
                policy.RequireRole("admin");
                 //   .RequireUserName().RequireClaim();
             }));
            services.AddAuthorization(authorization =>

            authorization.AddPolicy("is-user", policy =>
            {
                policy.RequireRole("user");
            }));
            return services;
        }
    }
}
