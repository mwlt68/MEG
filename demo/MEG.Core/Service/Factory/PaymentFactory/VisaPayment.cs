using System.Reflection;
using MEG.FactoryBase;

namespace MEG.Core.Service.Factory.PaymentFactory;

[FactoryModelIdentifier<CreditCardTypes>(CreditCardTypes.Visa)]
public class VisaPayment: PaymentBase
{
    public override string Pay(int amount)
    {
        // other operations
        var attribute = typeof(VisaPayment).GetCustomAttribute<FactoryModelIdentifierAttribute<CreditCardTypes>>(true);
        return FactoryHelper.GetPaymentMessage(amount,attribute.Identity);
    }
}