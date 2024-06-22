namespace MEG.Demo.ElasticLogger.Api.Models;

public class ApiBaseResponse<T>:ApiBaseResponse
{
    public T Data { get; set; }

    public ApiBaseResponse(T data)
    {
        Data = data;
        IsSuccess = true;
    }
}

public class ApiBaseResponse
{
    public bool IsSuccess { get; set; }
    public string? MessageCode { get; set; }
    public string Message { get; set; }
}

