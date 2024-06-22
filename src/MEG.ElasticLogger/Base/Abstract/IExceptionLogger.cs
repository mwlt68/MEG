using Microsoft.AspNetCore.Http;
using Nest;

namespace MEG.ElasticLogger.Base.Abstract;

public interface IExceptionLogger
{
    Task<IndexResponse?> AddAsync(HttpContext context, Exception exception, int statusCode,
        string? messageCode = null);
}