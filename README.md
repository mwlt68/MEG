# MEG

MEG is a solution that includes libraries that I think will be useful for .NET developers and that I have developed by putting my experience into use.

## 1)  MEG.FactoryBase

It is a library that enables the Factory Design Pattern to be implemented in a generic way and used practically with a clean coding structure.

### 1.1) Library Code Details

You can examine in detail how the library codes work by reading my [Factory Design Pattern in MEG Library](https://medium.com/p/1b4ae86a7464) article.

### 1.2) Implementation

First, include the **MEG.FactoryBase** library into your project via the Nuget Package Manager.

The AddFactoryBase method must be called to inject classes in Program.cs.
We can provide Assembly information where classes derived from FactoryBase will be located with FactoryBaseSettings.

```csharp

var factoryBaseSettings = FactoryBaseSettings.GetFactoryBaseSettings<PaymentFactory>();
builder.Services.AddFactoryBase(factoryBaseSettings);

```

If we rewrite the code structure below using FactoryBase.

```csharp
void Pay(CreditCardTypes creditCardType)
{
    switch (creditCardType)
    {
        case CreditCardTypes.Mastercard:
            PayWithMastercard();
            break;
        case CreditCardTypes.Visa:
            PayWithVisa();
            break;
    }
}

void PayWithMastercard()
{
    
}
void PayWithVisa()
{
    
}
```

The PaymentBase class that derives from FactoryBaseModel must be created.

```csharp
public abstract class PaymentBase: FactoryBaseModel<CreditCardTypes>
{
    public abstract string Pay(int amount);
}
```

Classes derived from PaymentBase and marked with the FactoryModelIdentifier attribute must be created.

```csharp

[FactoryModelIdentifier<CreditCardTypes>(CreditCardTypes.Mastercard)]

public class MastercardPayment : PaymentBase
{
    public override string Pay(int amount)
    {
        // other operations
        return $"{amount} TRY paid with Mastercard"
    }
}

[FactoryModelIdentifier<CreditCardTypes>(CreditCardTypes.Visa)]
public class VisaPayment: PaymentBase
{
    public override string Pay(int amount)
    {
        // other operations
        return $"{amount} TRY paid with Visa card"
    }
}

```

The PaymentFactory class that derives from FactoryBase must be created.

```csharp

public class PaymentFactory : FactoryBase<PaymentBase,CreditCardTypes>
{
}

```

If we want to use it in the Controller, the Pay method in the PaymentFactory is called when it comes from the service.

```csharp

public IActionResult GetPayment([FromServices] PaymentFactory paymentFactory,[FromQuery] CreditCardTypes creditCardType , [FromQuery] int amount = 15)
{
    var paymentResult = paymentFactory.CreateBaseModel(creditCardType)!.Pay(amount);
    return Ok(paymentResult);
}
```





