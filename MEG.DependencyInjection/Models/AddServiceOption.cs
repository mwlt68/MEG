using System.Reflection;

namespace MEG.DependencyInjection.Models;

public class AddServiceOption
{
    public Assembly Assembly { get; set; } = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
    public IEnumerable<Type> IgnoredTypes { get; set; } = [];
    public bool IsAutoInjectActive { get; set; }
    public bool IsOnlyBaseServiceAutoInject { get; set; }
}
