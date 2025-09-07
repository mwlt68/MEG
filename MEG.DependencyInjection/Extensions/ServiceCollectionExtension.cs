using System.Collections.Concurrent;
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

        AddLibraryServices(services, option);

        var serviceTypes = GetServiceTypes(option);
        using var serviceProvider = services.BuildServiceProvider();
        foreach (var serviceType in serviceTypes)
            services.AddServices(serviceProvider, serviceType, option);

        return services;
    }

    private static List<Type> GetServiceTypes(AddServiceOption option)
    {
        var serviceTypes = option.Assembly.GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false } && typeof(IBaseService).IsAssignableFrom(type))
            .Where(type => !option.IgnoredTypes.Any(ignoredType => ignoredType.IsAssignableFrom(type)))
            .ToList();
        return serviceTypes;
    }

    private static void AddLibraryServices(IServiceCollection services, AddServiceOption option)
    {
        services.AddSingleton(option);

        services.AddSingleton<PropertyInjectionService>();

        services.AddSingleton<SingletonServiceRegistrar>();
        services.AddSingleton<ScopedServiceRegistrar>();
        services.AddSingleton<TransientServiceRegistrar>();

        if (option.IsPropertyInjectionActive)
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, PropertyInjectingControllerActivator>());
    }

    private static void AddServices(this IServiceCollection services, ServiceProvider serviceProvider, Type serviceType,
        AddServiceOption option)
    {
        var serviceInterface = GetServiceInterface(serviceType);
        var serviceKey = GetServiceKey(serviceType);
        var registrar = GetRegistrar(serviceProvider, serviceType, option);

        registrar.Register(services, serviceType, serviceInterface, serviceKey);
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

    private static IServiceRegistrarBase GetRegistrar(ServiceProvider serviceProvider, Type serviceType,
        AddServiceOption option)
    {
        // Get the base service interface (IScopedService, ITransientService, etc.)
        var markerNamespace = typeof(IBaseService).Namespace;
        var baseServiceInterface = serviceType.GetInterfaces()
            .FirstOrDefault(interfaceType => typeof(IBaseService).IsAssignableFrom(interfaceType) &&
                                             interfaceType != typeof(IBaseService) &&
                                             interfaceType != typeof(IKeyedService) &&
                                             interfaceType.Namespace == markerNamespace);

        // For keyed services, use the non-keyed version (IKeyedScopedService -> IScopedService)
        if (typeof(IKeyedService).IsAssignableFrom(baseServiceInterface))
        {
            baseServiceInterface = baseServiceInterface.GetInterfaces()
                .FirstOrDefault(interfaceType => typeof(IBaseService).IsAssignableFrom(interfaceType) &&
                                                 !typeof(IKeyedService).IsAssignableFrom(interfaceType) &&
                                                 interfaceType != typeof(IBaseService) &&
                                                 interfaceType.Namespace == markerNamespace);
        }

        if (baseServiceInterface == null)
            throw new Exception($"Service type {serviceType.Name} does not implement any valid base service interface");

        var registrarType = GetRegistrarType(option, baseServiceInterface);

        if (registrarType == null)
        {
            throw new ArgumentOutOfRangeException(nameof(serviceType),
                $"No registrar found for service type: {baseServiceInterface.Name}");
        }

        return (IServiceRegistrarBase) serviceProvider.GetRequiredService(registrarType);
    }

    private static readonly ConcurrentDictionary<Type, Type?> RegistrarCache = new();

    private static Type? GetRegistrarType(AddServiceOption option, Type baseServiceInterface)
    {
        return RegistrarCache.GetOrAdd(baseServiceInterface, key =>
        {
            var genericRegistrarType = typeof(IServiceRegistrar<>).MakeGenericType(key);
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(type => type is { IsInterface: false, IsAbstract: false } &&
                                        type.GetInterfaces().Contains(genericRegistrarType));
        });
    }
}
