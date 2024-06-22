using System.Net;
using ElasticLogger.Api.Models;
using MEG.Demo.ElasticLogger.Api.Models;
using MEG.ElasticLogger.Base.Abstract;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MEG.Demo.ElasticLogger.Api.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IExceptionLogger _exceptionLogger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, IExceptionLogger exceptionLogger)
    {
        _next = next;
        _exceptionLogger = exceptionLogger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            context.Request.EnableBuffering();
            await _next(context);
        }
        catch (Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = new ApiBaseResponse();
            switch (exception)
            {
                case BadRequestException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case EntityNotFoundException e:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            if (exception is CustomException customException)
            {
                responseModel.MessageCode = customException.MessageCode;
                responseModel.Message = ExceptionMessageHelper.GetMessage(responseModel.MessageCode);
            }

            responseModel.IsSuccess = false;

            await _exceptionLogger.AddAsync(context, exception, response.StatusCode, responseModel.MessageCode);
            var result = JsonSerializer.Serialize(responseModel);
            await response.WriteAsync(result);
        }
    }
}