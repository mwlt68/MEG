using MEG.Demo.WebApi.Model;
using MEG.FactoryBase;

namespace MEG.Demo.WebApi.Service.Factory.PaymentFactory;

public abstract class PaymentBase: FactoryBaseModel<CreditCardTypes>
{
    public abstract string Pay(int amount);
}