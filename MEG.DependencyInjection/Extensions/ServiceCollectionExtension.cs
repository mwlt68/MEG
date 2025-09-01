using MEG.DependencyInjection.Activators;
using MEG.DependencyInjection.Models;
using MEG.DependencyInjection.ServiceRegistrar.Concretes;
using MEG.DependencyInjection.ServiceRegistrar.Interfaces;
using MEG.DependencyInjection.Services;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MEG.DependencyInjection.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services, AddServiceOption? option = null)
    {
        option ??= new AddServiceOption();

        services.AddSingleton(option);
        services.AddSingleton<PropertyInjectionService>();
        services.AddSingleton<SingletonServiceRegistrar>();
        services.AddSingleton<ScopedServiceRegistrar>();
        services.AddSingleton<TransientServiceRegistrar>();

        var serviceTypes = option.Assembly.GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false } && typeof(IBaseService).IsAssignableFrom(type))
            .Where(type => !option.IgnoredTypes.Any(ignoredType => ignoredType.IsAssignableFrom(type)))
            .ToList();

        foreach (var serviceType in serviceTypes)
            services.AddServices(serviceType, option);

        if (option.IsAutoInjectActive)
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, PropertyInjectingControllerActivator>());

        return services;
    }

    private static void AddServices(this IServiceCollection services, Type serviceType, AddServiceOption option)
    {
        var serviceInterface = GetServiceInterface(serviceType);
        var serviceKey = GetServiceKey(serviceType);
        var registrar = GetRegistrar(services,serviceType);

        registrar.Register(services, serviceType, serviceInterface, serviceKey,
            option.IsAutoInjectActive);
    }

    private static Type? GetServiceInterface(Type serviceType)
    {
        var markerNamespace = typeof(IBaseService).Namespace;

        return serviceType
            .GetInterfaces()
            .FirstOrDefault(type => type != serviceType && type.Namespace != markerNamespace);
    }

    private static object? GetServiceKey(Type serviceType)
    {
        var isKeyedService = typeof(IKeyedService).IsAssignableFrom(serviceType);
        if (!isKeyedService)
            return null;

        var instance = Activator.CreateInstance(serviceType);

        var property = serviceType.GetProperty(nameof(IKeyedService.ServiceKey));

        return property?.GetValue(instance);
    }

    private static IServiceRegistrarBase GetRegistrar( IServiceCollection services,Type serviceType)
    {
        // Get the base service interface (IScopedService, ITransientService, etc.)
        var markerNamespace = typeof(IBaseService).Namespace;
        var baseServiceInterface = serviceType.GetInterfaces()
            .First(interfaceType => typeof(IBaseService).IsAssignableFrom(interfaceType) &&
                                    interfaceType != typeof(IBaseService) &&
                                    interfaceType != typeof(IKeyedService) &&
                                    interfaceType.Namespace == markerNamespace);

        // For keyed services, use the non-keyed version (IKeyedScopedService -> IScopedService)
        if (typeof(IKeyedService).IsAssignableFrom(baseServiceInterface))
        {
            baseServiceInterface = baseServiceInterface.GetInterfaces()
                .First(interfaceType => typeof(IBaseService).IsAssignableFrom(interfaceType) &&
                                        !typeof(IKeyedService).IsAssignableFrom(interfaceType) &&
                                        interfaceType != typeof(IBaseService) &&
                                        interfaceType.Namespace == markerNamespace);
        }

        // Find registrar for the base service interface
        var genericRegistrarType = typeof(IServiceRegistrar<>).MakeGenericType(baseServiceInterface);
        var registrarType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type is { IsInterface: false, IsAbstract: false })
            .FirstOrDefault(type => type.GetInterfaces().Contains(genericRegistrarType));

        if (registrarType == null)
            throw new ArgumentOutOfRangeException(nameof(serviceType),
                $"No registrar found for service type: {baseServiceInterface.Name}");

        using var serviceProvider = services.BuildServiceProvider();
        return (IServiceRegistrarBase)serviceProvider.GetRequiredService(registrarType);
    }
}
