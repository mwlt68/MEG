using System.Security.Claims;
using MEG.ElasticLogger.Base.Abstract;
using Microsoft.AspNetCore.Http;
using Nest;

namespace MEG.ElasticLogger.Base.Concrete;

public class ElasticLogger : IElasticLogger
{
    private readonly IHttpContextAccessor _contextAccessor;

    public ElasticLogger(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }
    
    public string? GetOperatorId()
    {
        return _contextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
            ?.Value;
    }

    public Task HandleIndexResponseAsync(IndexResponse? indexResponse)
    {
        return Task.CompletedTask;
    }
}