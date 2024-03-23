using Microsoft.Extensions.DependencyInjection;

namespace MEG.FactoryBase;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddFactoryBase(this IServiceCollection services,FactoryBaseSettings? factoryBaseSettings = null, ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        var factoryBaseTypes = new List<Type>() { typeof(FactoryBase<,>) ,typeof(FactoryBase<,,>)};
        
        factoryBaseSettings = factoryBaseSettings ?? new FactoryBaseSettings();
        var assembly = FactoryBaseSettings.FactoriesAssembly;
        
        var factories = assembly.GetExportedTypes()
            .Where(t => t.BaseType is { IsGenericType: true } &&
                        factoryBaseTypes.Contains(t.BaseType.GetGenericTypeDefinition()) &&
                        !factoryBaseTypes.Contains(t))
            .ToArray();
        
        foreach (var factory in factories)
        {
            services.Add(new ServiceDescriptor(factory, factory, lifetime));
        }

        services.AddSingleton(factoryBaseSettings);

        return services;
    }
    
}