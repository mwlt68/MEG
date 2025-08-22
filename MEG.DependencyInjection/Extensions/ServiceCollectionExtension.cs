using System.Reflection;
using MEG.DependencyInjection.ServiceRegistrar.Interfaces;
using MEG.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MEG.DependencyInjection.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
        var serviceTypes = assembly.GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false } && typeof(IBaseService).IsAssignableFrom(type))
            .ToList();

        foreach (var serviceType in serviceTypes)
            services.AddServices(serviceType);

        return services;
    }
    private static void AddServices(this IServiceCollection services,Type serviceType)
    {
        var markerNamespace = typeof(IBaseService).Namespace;

        var serviceKey = GetServiceKey(serviceType);

        var interfaces = serviceType.GetInterfaces()
            .Where(type => type != serviceType && type.Namespace != markerNamespace)
            .ToList();

        var serviceInterface = serviceType.GetInterfaces()
            .First(type => typeof(IBaseService).IsAssignableFrom(type) && type != typeof(IBaseService) &&
                           type != typeof(IKeyedBaseService) && type.Namespace == markerNamespace);

        var registrar = GetRegistrar(serviceInterface);

        if (interfaces.Any())
        {
            foreach (var @interface in interfaces)
                registrar.Register(services, @interface, serviceType, serviceKey);
        }
        else
            registrar.Register(services, serviceType, serviceKey);
    }

    private static object? GetServiceKey(Type serviceType)
    {
        object? serviceKey = null;
        if (typeof(IKeyedBaseService).IsAssignableFrom(serviceType))
        {
            var instance = Activator.CreateInstance(serviceType);
            var property = serviceType.GetProperty(nameof(IKeyedBaseService.ServiceKey));
            serviceKey = property?.GetValue(instance);
        }

        return serviceKey;
    }

    private static IServiceRegistrarBase GetRegistrar(Type serviceType)
    {
        var genericRegistrarType = typeof(IServiceRegistrar<>).MakeGenericType(serviceType);

        var registrarType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .FirstOrDefault(type =>
                genericRegistrarType.IsAssignableFrom(type) && type is { IsInterface: false, IsAbstract: false });

        if (registrarType == null)
            throw new ArgumentOutOfRangeException(nameof(serviceType));

        return (IServiceRegistrarBase) Activator.CreateInstance(registrarType)!;
    }
}
