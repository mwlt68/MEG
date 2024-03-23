using MEG.Demo.WebApi.Model;
using MEG.Demo.WebApi.Service.Factory.PaymentFactory;
using Microsoft.AspNetCore.Mvc;

namespace MEG.Demo.WebApi.Controller;

[ApiController]
[Route("[controller]")]
public class FactoryBaseDemoController : ControllerBase
{
    private PaymentFactory _paymentFactory { get; set; }
    public FactoryBaseDemoController(PaymentFactory paymentFactory)
    {
        _paymentFactory = paymentFactory;
    }
    
    [HttpGet("payment")]
    public IActionResult GetPayment([FromQuery] CreditCardTypes creditCardType , [FromQuery] int amount = 15)
    {
        var paymentResult = _paymentFactory.CreateBaseModel(creditCardType)!.Pay(amount);
        return Ok(paymentResult);
    }
}