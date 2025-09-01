using System.Reflection;
using MEG.DependencyInjection.Attributes;
using MEG.DependencyInjection.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MEG.DependencyInjection.Services;

public class PropertyInjectionService(AddServiceOption addServiceOption)
{
    public void Inject(object target, IServiceProvider serviceProvider)
    {
        if (!addServiceOption.IsAutoInjectActive)
            return;

        var properties = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            var currentValue = property.GetValue(target);
            if (currentValue != null)
                continue;

            var ignoreAttribute = property.GetCustomAttribute<IgnoreInjectionAttribute>();

            if (!property.CanWrite || ignoreAttribute != null)
                continue;

            if (addServiceOption.IsOnlyBaseServiceAutoInject)
            {
                var isBaseService = typeof(IBaseService).IsAssignableFrom(property.PropertyType);
                if (!isBaseService)
                    continue;
            }

            object? service;

            var isKeyedService = typeof(IKeyedService).IsAssignableFrom(property.PropertyType);

            if (isKeyedService)
            {
                var serviceKeyAttribute = property.GetCustomAttribute<AutoKeyedAttribute>();

                if (serviceKeyAttribute == null)
                {
                    throw new InvalidOperationException(
                        $"Property {property.Name} of type {target.GetType().Name} is marked as IKeyedService but does not have a ServiceKeyAttribute.");
                }

                var serviceKey = serviceKeyAttribute.Key;

                var getKeyedServiceMethod = typeof(ServiceProviderKeyedServiceExtensions)
                    .GetMethod(nameof(ServiceProviderKeyedServiceExtensions.GetKeyedService))
                    ?.MakeGenericMethod(property.PropertyType);

                service = getKeyedServiceMethod?.Invoke(null, [serviceProvider, serviceKey]);
            }
            else
                service = serviceProvider.GetService(property.PropertyType);

            if (service != null)
                property.SetValue(target, service);
        }
    }
}
