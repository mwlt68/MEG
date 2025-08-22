using MEG.DependencyInjection.Models;
using MEG.DependencyInjection.ServiceRegistrar.Interfaces;
using MEG.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MEG.DependencyInjection.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services, AddServiceOption? options = null)
    {
        options ??= new AddServiceOption();

        var serviceTypes = options.Assembly.GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false } && typeof(IBaseService).IsAssignableFrom(type))
            .Where(type => !options.IgnoredTypes.Any(ignoredType => ignoredType.IsAssignableFrom(type)))
            .ToList();

        foreach (var serviceType in serviceTypes)
            services.AddServices(serviceType);

        return services;
    }

    private static void AddServices(this IServiceCollection services, Type serviceType)
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
            registrar.Register(services, interfaces.First(), serviceType, serviceKey);
        else
            registrar.Register(services, serviceType, serviceKey);
    }

    private static object? GetServiceKey(Type serviceType)
    {
        var isKeyedService = typeof(IKeyedBaseService).IsAssignableFrom(serviceType);
        if (!isKeyedService)
            return null;

        var instance = Activator.CreateInstance(serviceType);

        var property = serviceType.GetProperty(nameof(IKeyedBaseService.ServiceKey));

        return property?.GetValue(instance);
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
