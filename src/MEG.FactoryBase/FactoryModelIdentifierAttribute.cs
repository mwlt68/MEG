namespace MEG.FactoryBase;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public abstract class FactoryModelIdentifierAttribute<TIdentity>(TIdentity identity) : Attribute
{
    public TIdentity Identity { get; set; } = identity;
}