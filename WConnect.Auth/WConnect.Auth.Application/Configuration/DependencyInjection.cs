using System.Data;
using WConnect.Auth.Application.Builders;
using WConnect.Auth.Core.Builders;
using WConnect.Auth.Core.Repositories;
using WConnect.Auth.Database.Repositories;
using Microsoft.Extensions.DependencyInjection;
using WConnect.Auth.Application.Providers;
using WConnect.Auth.Core.Providers;
using WConnect.Auth.Database;

namespace WConnect.Auth.Application.Configuration;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<IUserBuilder,UserBuilder>();
        services.AddScoped<IUserBuilder,UserBuilder>();
        services.AddScoped<ITimeProvider, TimeProvider>();
        services.AddTransient<IDbConnection>(_ => MySqlConnectionFactory.Create());
        return services;
    }
}