using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Domain.Helpers;

public static class DependencyInjectionHelper
{
    public static void AddAssemblyServices(IServiceCollection services, Assembly domainAssembly, Assembly concreeteAssembly)
    {
        // Obtener las interfaces del ensamblado del proyecto de dominio
        var interfaces = domainAssembly?
            .ExportedTypes
            .Where(t => t.IsInterface)
            .ToList();

        // Obtener las clases del ensamblado del proyecto que implementa las interfaces
        var classes = concreeteAssembly?
            .ExportedTypes
            .Where(t => t.IsClass)
            .ToList();

        // Recorrer cada clase y su interfaz correspondiente<
        if (interfaces != null && classes != null)
        {
            foreach (var @class in classes)
            {
                var implementedInterfaces = @class.GetInterfaces();
                foreach (var implementedInterface in implementedInterfaces)
                {
                    if (interfaces.Contains(implementedInterface))
                    {
                        services.AddTransient(implementedInterface, @class);
                    }
                }
            }
        }
    }
}