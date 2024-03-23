using MEG.FactoryBase;

namespace MEG.Demo.WebApi.Service.Factory.NotificationFactory;

[FactoryModelIdentifier<string>("Sms")]

public class SmsNotification : NotificationBase
{
    public override string Notify()
    {
        return "This is Sms notification";
    }
}