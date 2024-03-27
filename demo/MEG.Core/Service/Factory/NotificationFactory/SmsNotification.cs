using System.Reflection;
using MEG.FactoryBase;

namespace MEG.Core.Service.Factory.NotificationFactory;

[FactoryModelIdentifier<string>("Sms")]

public class SmsNotification : NotificationBase
{
    public override string Notify()
    {
        // other operations
        var attribute = typeof(SmsNotification).GetCustomAttribute<FactoryModelIdentifierAttribute<string>>(true);
        return FactoryHelper.GetNotificationMessage(attribute?.Identity);
    }
}