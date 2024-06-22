namespace MEG.Demo.ElasticLogger.Api.Models;

public abstract class CustomException:Exception
{
    protected CustomException(string? messageCode)
    {
        MessageCode = messageCode;
    }

    public string? MessageCode { get; set; }
}

public class EntityNotFoundException : CustomException
{
    public EntityNotFoundException(string? messageCode) : base(messageCode)
    {
    }
}

public class BadRequestException : CustomException
{
    public BadRequestException(string? messageCode) : base(messageCode)
    {
    }
}