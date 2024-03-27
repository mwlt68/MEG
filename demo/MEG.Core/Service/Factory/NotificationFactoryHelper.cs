using MEG.Core.Service.Factory.PaymentFactory;

namespace MEG.Core.Service.Factory;

public static class FactoryHelper
{
    public static string GetNotificationMessage(string notificationType)=> $"This is {notificationType.ToLower()} notification";
    public static string GetPaymentMessage(int amount,CreditCardTypes cardType)=> $"{amount} TRY paid with {cardType.ToString().ToLower()} card"; 
}