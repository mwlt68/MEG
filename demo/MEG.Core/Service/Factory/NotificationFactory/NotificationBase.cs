using MEG.FactoryBase;

namespace MEG.Core.Service.Factory.NotificationFactory;

public abstract class NotificationBase:FactoryBaseModel<string>
{
    public abstract string Notify();
}