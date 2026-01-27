using Domain.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Adapters;

public static class AdapterServiceRegistration
{
    public static IServiceCollection AddAdapterServices(this IServiceCollection services)
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