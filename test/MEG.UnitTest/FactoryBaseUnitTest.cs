using MEG.Core.Service.Factory;
using MEG.Core.Service.Factory.NotificationFactory;
using MEG.Core.Service.Factory.PaymentFactory;
using MEG.FactoryBase;
using Microsoft.Extensions.DependencyInjection;

namespace MEG.UnitTest;

public class FactoryBaseUnitTest
{
    private readonly NotificationFactory _notificationFactory;
    private readonly PaymentFactory _paymentFactory;
    public FactoryBaseUnitTest()
    { 
        var serviceCollection = new ServiceCollection();
        var factorySettings = FactoryBaseSettings.GetFactoryBaseSettings<PaymentFactory>();
        serviceCollection.AddFactoryBase(factorySettings);
        _notificationFactory = new NotificationFactory();
        _paymentFactory = new PaymentFactory();
    }
    
    [Theory]
    [InlineData("sms")]
    [InlineData("SMS")]
    [InlineData("Mail")]
    [InlineData("MaiL")]
    public void Notify_AcceptedNotificationType_CorrectMessage(string notificationType)
    {
        var notification = _notificationFactory.CreateBaseModel(notificationType)!.Notify();
        var actualNotification = FactoryHelper.GetNotificationMessage(notificationType);
        
        Assert.Equal(notification,actualNotification);
    }

    [Theory]
    [InlineData("slack")]
    public void Notify_OutOfNotificationType_ApplicationException(string notificationType)
    {
        string NotifyFunc() => _notificationFactory.CreateBaseModel(notificationType)!.Notify();

        Assert.Throws<ApplicationException>(NotifyFunc);
    }
    
    
    [Theory]
    [InlineData(CreditCardTypes.Visa,10)]
    [InlineData(CreditCardTypes.Mastercard,13)]
    public void Pay_AcceptedCreditCardType_CorrectMessage(CreditCardTypes creditCardType,int amount)
    {
        var paymentResult = _paymentFactory.CreateBaseModel(creditCardType)!.Pay(amount);
        var actualNotification = FactoryHelper.GetPaymentMessage(amount,creditCardType);
        
        Assert.Equal(paymentResult,actualNotification);
    }

    [Theory]
    [InlineData(CreditCardTypes.Troy,12)]
    public void Pay_OutOfCreditCardType_ArgumentException(CreditCardTypes creditCardType,int amount)
    {
        string NotifyFunc() => _paymentFactory.CreateBaseModel(creditCardType)!.Pay(amount);

        Assert.Throws<ArgumentException>(NotifyFunc);
    }
    
    [Theory]
    [InlineData(CreditCardTypes.Troy,12)]
    public void Pay_CustomException_ArgumentNullException(CreditCardTypes creditCardType,int amount)
    {
        var exception = new ArgumentNullException();
        
        string NotifyFunc() => _paymentFactory.CreateBaseModel(creditCardType, exception: exception)!.Pay(amount);

        Assert.Throws<ArgumentNullException>(NotifyFunc);
    }
    
    [Theory]
    [InlineData(CreditCardTypes.Troy)]
    public void CreateBaseModel_BlockThrowException_Null(CreditCardTypes creditCardType)
    {
        var paymentFactoryBase =  _paymentFactory.CreateBaseModel(creditCardType, throwException: false);
        
        Assert.Null(paymentFactoryBase);
    }
}