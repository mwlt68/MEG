using MEG.Demo.WebApi.Service.Factory.NotificationFactory;
using MEG.Demo.WebApi.Service.Factory.PaymentFactory;
using Microsoft.AspNetCore.Mvc;

namespace MEG.Demo.WebApi.Controller;

[ApiController]
[Route("[controller]")]
public class FactoryBaseDemoController : ControllerBase
{
    public FactoryBaseDemoController()
    {
    }
    
    [HttpGet("payment")]
    public IActionResult GetPayment([FromServices] PaymentFactory paymentFactory,[FromQuery] CreditCardTypes creditCardType , [FromQuery] int amount = 15)
    {
        var paymentResult = paymentFactory.CreateBaseModel(creditCardType)!.Pay(amount);
        return Ok(paymentResult);
    }
    
    [HttpGet("notification")]
    public IActionResult GetNotification([FromServices]NotificationFactory notificationFactory,[FromQuery] string notificationType)
    {
        var paymentResult = notificationFactory.CreateBaseModel(notificationType)!.Notify();
        return Ok(paymentResult);
    }
}