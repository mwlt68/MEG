using Nest;

namespace MEG.ElasticLogger.Base.Abstract;

public interface IElasticLogger
{
    Task HandleIndexResponseAsync(IndexResponse? indexResponse);
    string? GetOperatorId();
}