using System.Reflection;
using MEG.FactoryBase;

namespace MEG.Core.Service.Factory.PaymentFactory;

[FactoryModelIdentifier<CreditCardTypes>(CreditCardTypes.Mastercard)]

public class MastercardPayment : PaymentBase
{
    public override string Pay(int amount)
    {
        // other operations
        var attribute = typeof(MastercardPayment).GetCustomAttribute<FactoryModelIdentifierAttribute<CreditCardTypes>>(true);
        return FactoryHelper.GetPaymentMessage(amount,attribute.Identity);
    }
}