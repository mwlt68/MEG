using MEG.Demo.WebApi.Model;
using MEG.FactoryBase;

namespace MEG.Demo.WebApi.Service.Factory.PaymentFactory;

[FactoryModelIdentifier<CreditCardTypes>(CreditCardTypes.Visa)]
public class VisaPayment: PaymentBase
{
    public override string Pay(int amount)
    {
        return $"{amount} pay by visa"; 
    }
}