using Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using System.Reflection;

namespace Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TeamTasksSampleContext>(options => options.UseSqlServer(configuration.GetConnectionString("TeamTasksSampleContexto")));

        // Invocar el helper para registrar servicios
        DependencyInjectionHelper.AddAssemblyServices(
            services,
            Assembly.Load("Domain"), // Ensamblado del proyecto de dominio
            Assembly.GetExecutingAssembly() // Ensamblado actual
        );

        return services;
    }
}