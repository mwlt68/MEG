using System.Reflection;

namespace MEG.DependencyInjection.Models;

public class AddServiceOption
{
    private Assembly? _assembly;

    public Assembly Assembly
    {
        get => _assembly ??= Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
        set => _assembly = value ?? throw new ArgumentNullException(nameof(value));
    }
    public IEnumerable<Type> IgnoredTypes { get; set; } = [];
    public bool IsPropertyInjectionActive { get; set; }
    public bool IsOnlyBaseServiceInject { get; set; }
}
