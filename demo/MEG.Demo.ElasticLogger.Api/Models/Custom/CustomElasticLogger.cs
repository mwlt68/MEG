using System.Security.Claims;
using MEG.ElasticLogger.Base.Abstract;
using Nest;

namespace MEG.Demo.ElasticLogger.Api.Models.Custom;

public class CustomElasticLogger(IHttpContextAccessor contextAccessor) : IElasticLogger
{
    public string? GetOperatorId()
    {
        return contextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
            ?.Value;
    }

    public Task HandleIndexResponseAsync(IndexResponse? indexResponse)
    {
        return Task.CompletedTask;
    }
}