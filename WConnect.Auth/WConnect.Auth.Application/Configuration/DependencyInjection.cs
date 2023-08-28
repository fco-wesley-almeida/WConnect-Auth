using System.Data;
using WConnect.Auth.Application.Builders;
using WConnect.Auth.Core.Builders;
using WConnect.Auth.Core.Repositories;
using WConnect.Auth.Database.Repositories;
using WConnect.Auth.Application.Providers;
using WConnect.Auth.Application.Services;
using WConnect.Auth.Core.Providers;
using WConnect.Auth.Core.Services;
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
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddTransient<IDbConnection>(_ => MySqlConnectionFactory.Create());
        services.AddScoped<IJwtTokenGeneratorService, JwtTokenGeneratorService>();
        services.AddTransient<IClaimsIdentityBuilder, ClaimsIdentityBuilder>();
        services.AddTransient<IJwtTokenBuilder, JwtTokenBuilder>();
        services.AddTransient<ISecurityTokenDescriptorBuilder, SecurityTokenDescriptorBuilder>();
        return services;
    }
}