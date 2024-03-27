using System.Reflection;
using MEG.FactoryBase;

namespace MEG.Core.Service.Factory.NotificationFactory;

[FactoryModelIdentifier<string>("Mail")]
public class MailNotification : NotificationBase
{
    public override string Notify()
    {
        // other operations
        var attribute = typeof(MailNotification).GetCustomAttribute<FactoryModelIdentifierAttribute<string>>(true);
        return FactoryHelper.GetNotificationMessage(attribute?.Identity);
    }
    
}