namespace MEG.FactoryBase;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class FactoryModelIdentifierAttribute<TIdentity> : Attribute
{
    public TIdentity Identity { get; set; }

    public FactoryModelIdentifierAttribute(TIdentity identity)
    {
        this.Identity = identity;
    }
}