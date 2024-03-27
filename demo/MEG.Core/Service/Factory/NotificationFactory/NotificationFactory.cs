using MEG.FactoryBase;
namespace MEG.Core.Service.Factory.NotificationFactory;

public class NotificationFactory : FactoryBase<NotificationBase,string,ApplicationException>
{
    protected override ApplicationException DefaultException => throw new System.ApplicationException("Test Exception");
}