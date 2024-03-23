using System.Reflection;

namespace MEG.FactoryBase;

public class FactoryBaseSettings
{
    private static Assembly _factoriesAssembly { get; set; }
    private static Assembly _factoryModelsAssembly { get; set; }
    public static Assembly FactoriesAssembly => _factoriesAssembly;
    public static Assembly FactoryModelsAssembly => _factoryModelsAssembly;

    public FactoryBaseSettings(Assembly factoriesAssembly,Assembly factoryModelsAssembly)
    {
        _factoriesAssembly = factoriesAssembly;
        _factoryModelsAssembly = factoryModelsAssembly;
    }

    public FactoryBaseSettings(Assembly? assembly = null)
    {
        assembly ??= Assembly.GetExecutingAssembly();
        _factoriesAssembly = assembly;
        _factoryModelsAssembly = assembly;
    }
    
    public static FactoryBaseSettings GetFactoryBaseSettings<TAssembly>()
    {
        var assembly = typeof(TAssembly).Assembly;
        return new FactoryBaseSettings(assembly);
    }
        
}