using System.Text;
using Microsoft.AspNetCore.Http;

namespace MEG.ElasticLogger.Extensions;

public static class HttpRequestExtension
{
    public static async Task<string> GetRequestBodyAsync(this HttpRequest request, Encoding? encoding = null)
    {
        string requestBody;
        request.Body.Position = 0; 
        using (var reader = new StreamReader(request.Body, encoding ?? Encoding.UTF8, true, 1024, true))
        {
            requestBody = await reader.ReadToEndAsync().ConfigureAwait(false);
        }
        return requestBody;
    }

}