using MEG.FactoryBase;

namespace MEG.Core.Service.Factory.PaymentFactory;

public abstract class PaymentBase: FactoryBaseModel<CreditCardTypes>
{
    public abstract string Pay(int amount);
}