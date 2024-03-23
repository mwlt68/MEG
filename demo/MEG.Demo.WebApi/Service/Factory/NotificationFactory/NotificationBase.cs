using MEG.FactoryBase;

namespace MEG.Demo.WebApi.Service.Factory.NotificationFactory;

public abstract class NotificationBase:FactoryBaseModel<string>
{
    public abstract string Notify();
}