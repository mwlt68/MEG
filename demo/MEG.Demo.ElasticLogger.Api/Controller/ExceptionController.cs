using MEG.Demo.ElasticLogger.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace MEG.Demo.ElasticLogger.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptionController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetExceptionAsync([FromQuery] ExceptionType? exceptionType,[FromQuery]  string messageCode)
        {
            throw Handle(exceptionType,messageCode);
        }
        
        [HttpPost]
        public async Task<IActionResult> PostExceptionAsync([FromBody] BodyModel bodyModel)
        {
            throw Handle(bodyModel.exceptionType,bodyModel.messageCode);
        }
        
        [HttpPatch("patch/{exceptionType}/test/{messageCode}/path")]
        public async Task<IActionResult> PatchExceptionAsync(ExceptionType? exceptionType, string messageCode)
        {
            throw Handle(exceptionType,messageCode);
        }
        
        [HttpPost("PostFromFormException")]
        public async Task<IActionResult> PostFromFormExceptionAsync([FromForm] ExceptionType? exceptionType,[FromForm]  string messageCode)
        {
            throw Handle(exceptionType,messageCode);
        }
        
        [HttpPost("PostHeaderException")]
        public async Task<IActionResult> PostHeaderExceptionAsync([FromHeader] ExceptionType? exceptionType,[FromHeader]  string messageCode)
        {
            throw Handle(exceptionType,messageCode);
        }

        private Exception Handle(ExceptionType? exceptionType,  string messageCode)
        {
            switch (exceptionType)
            {
                case ExceptionType.EntityNotFound:
                    return new EntityNotFoundException(messageCode);
                case ExceptionType.BadRequest:
                    return new BadRequestException(messageCode);
                default:
                    return new Exception(messageCode);
            }
        }
    }
}
public enum ExceptionType
{
    EntityNotFound,
    BadRequest,
}

public class BodyModel
{
    public ExceptionType? exceptionType { get; set; }
    public string messageCode { get; set; }
}