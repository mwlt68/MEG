namespace MEG.DependencyInjection.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class AutoKeyedAttribute(object key) : Attribute
{
    public object Key { get; } = key;
}
