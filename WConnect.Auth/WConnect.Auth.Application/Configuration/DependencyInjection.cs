using System.Data;
using System.IdentityModel.Tokens.Jwt;
using WConnect.Auth.Application.Builders;
using WConnect.Auth.Database.Repositories;
using WConnect.Auth.Application.Providers;
using WConnect.Auth.Application.Services;
using WConnect.Auth.Core;
using WConnect.Auth.Core.Database;
using WConnect.Auth.Core.UseCases.SignIn;
using WConnect.Auth.Core.UseCases.SignUp;
using WConnect.Auth.Database;

namespace WConnect.Auth.Application.Configuration;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>()!;
        
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<IUserBuilder,UserBuilder>();
        services.AddScoped<IUserBuilder,UserBuilder>();
        services.AddScoped<ITimeProvider, TimeProvider>();
        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<IJwtTokenGeneratorService, JwtTokenGeneratorService>();

        
        services.AddTransient<IClaimsIdentityBuilder, ClaimsIdentityBuilder>();
        services.AddTransient<IJwtTokenBuilder, JwtTokenBuilder>();
        services.AddTransient<ISecurityTokenDescriptorBuilder, SecurityTokenDescriptorBuilder>();
        services.AddTransient<JwtSecurityTokenHandler>(_ => new JwtSecurityTokenHandler());
        services.AddTransient<IDbConnection>(_ => MySqlConnectionFactory.Create(configuration));
        
        return services;
    }
}