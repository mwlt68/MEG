namespace ElasticLogger.Api.Models;

public static class ExceptionMessageHelper
{
    public static string GetMessage(string? messageCode)
    {
        switch (messageCode)
        {
            case "ERR001" : return "Requested value not found.";
            case "ERR002" : return "Operation failed. Please try again later.";
            case "ERR003" : return "Invalid input. Please enter correct information.";
            case "ERR004" : return "Server error. Please contact your system administrator.";
            default: return "A technical error has occurred.";
        }
    }
}