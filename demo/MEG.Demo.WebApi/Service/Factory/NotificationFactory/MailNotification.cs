using MEG.FactoryBase;

namespace MEG.Demo.WebApi.Service.Factory.NotificationFactory;

[FactoryModelIdentifier<string>("Mail")]
public class MailNotification : NotificationBase
{
    public override string Notify()
    {
        return "This is mail notification";
    }
    
}