using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ProductionScheduler.Application.Security;
using ProductionScheduler.Core.Entities;

namespace ProductionScheduler.Infrastructure.Security
{
    internal static class Extensions
    {
        public static IServiceCollection AddSecurity(this IServiceCollection services)
        {
            services
                .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>()
                .AddSingleton<IPasswordManager, PasswordManager>();
            // singleton wystarczy to nie zawiera w sobie stanu aplikacji ktory zmienia po prostu odpowiada za dodatkowa logike infrastruktury #refactor

            return services;
        }
    }
}
