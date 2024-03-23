using MEG.FactoryBase;

namespace MEG.Demo.WebApi.Service.Factory.PaymentFactory;

[FactoryModelIdentifier<CreditCardTypes>(CreditCardTypes.Mastercard)]

public class MastercardPayment : PaymentBase
{
    public override string Pay(int amount)
    {
        return $"{amount} pay by mastercard";
    }
}