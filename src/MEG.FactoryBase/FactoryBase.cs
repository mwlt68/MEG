using System.Reflection;

namespace MEG.FactoryBase;


public abstract class FactoryBase<TFactoryBaseModel, TIdentifier, TException>()
    where TFactoryBaseModel : FactoryBaseModel<TIdentifier>
    where TException : Exception, new()
{
    private Type[]? _types;
    protected virtual TException DefaultException => new TException();

    private Type[] GetTypes => _types ??= FactoryBaseSettings.FactoryModelsAssembly.GetTypes()
        .Where(t => t.IsSubclassOf(typeof(TFactoryBaseModel)))
        .ToArray();


    public TFactoryBaseModel? CreateBaseModel(TIdentifier identifier, bool throwException = true,
        TException? exception = null)
    {
        foreach (var type in GetTypes)
        {
            var attributes = type.GetCustomAttributes<FactoryModelIdentifierAttribute<TIdentifier>>(false);
            
            var attribute = attributes.FirstOrDefault(a =>
            {
                if (a.Identity is string attributeStr && identifier is string identifierStr)
                {
                    return string.Equals(attributeStr, identifierStr, StringComparison.CurrentCultureIgnoreCase);
                }
                else
                {
                    return a.Identity?.Equals(identifier) == true;
                }
            });
            
            if (attribute is not null)
            {
                var factoryBaseModel = (TFactoryBaseModel?)Activator.CreateInstance(type);
                return factoryBaseModel;
            }
        }

        if (throwException)
        {
            if (exception is not null)
                throw exception;

            throw DefaultException;
        }

        return null;
    }
}

public abstract class
    FactoryBase<TBaseModel, TIdentifier>() : FactoryBase<TBaseModel, TIdentifier,
    ArgumentException>()
    where TBaseModel : FactoryBaseModel<TIdentifier>;