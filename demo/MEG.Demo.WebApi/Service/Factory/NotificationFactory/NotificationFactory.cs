using MEG.FactoryBase;

namespace MEG.Demo.WebApi.Service.Factory.NotificationFactory;

public class NotificationFactory : FactoryBase<NotificationBase,string,ApplicationException>
{
    protected override ApplicationException DefaultException => throw new ApplicationException("Test Exception");
    
}