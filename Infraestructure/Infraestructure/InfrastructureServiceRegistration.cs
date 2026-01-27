using Domain.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infraestructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Invocar el helper para registrar servicios
        DependencyInjectionHelper.AddAssemblyServices(
            services,
            Assembly.Load("Domain"), // Ensamblado del proyecto de dominio
            Assembly.GetExecutingAssembly() // Ensamblado actual
        );

        return services;
    }
}