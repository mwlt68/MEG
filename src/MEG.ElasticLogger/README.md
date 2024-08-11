#  MEG.ElasticLogger

This library allows you to quickly create the basic logging mechanism of your project.

<b>Key features</b>

- It is based on elastic search (Visualization, filtering, quick search etc. features can be easily used via Kibana).
- It has a customizable structure.
- Exceptions can be tracked during application operation.
- Requests coming to endpoints can be tracked.
- Entity changes can be tracked.

## 1- Implementation

- Include MEG.ElasticLogger library in your project

```shell
  dotnet add package MEG.ElasticLogger --version 1.0.0
```

- ElasticLoggerSettings configuration

```json
"ElasticLoggerSettings": {
    "ElasticSearchUrl": "http://localhost:9201",
    "IsActive": true,
    "IsAuditLoggerActive": true,
    "IsExceptionLoggerActive": true,
    "IsActionLoggerIndexActive": true,
    "ExceptionLoggerIndex": "exception-logger",
    "AuditLoggerIndex": "audit-logger",
    "ActionLoggerIndex": "action-logger"
}
```

Details of `ElasticLoggerSettings` in your project's appsettings are given below.

<b> ElasticSearchUrl</b> : Host address where ElasticSearch is running.


<b> IsActive</b> : Logs activity status.


<b> IsAuditLoggerActive </b>: AuditLogger activity status.


<b> IsExceptionLoggerActive </b>: ExceptionLogger activity status.


<b> IsActionLoggerIndexActive </b>: ActionLogger activity status.


<b> ExceptionLoggerIndex </b>: ExceptionLogger index name.


<b> AuditLoggerIndex </b>: AuditLogger index name.


<b> ActionLoggerIndex </b>: ActionLogger index name.


- Service Injection

The following code block injects the default classes belonging to elasticLogger. In the following section, we will talk about how you can customize it.

```csharp
builder.Services.AddElasticLogger(builder.Configuration);
```

## 2- Logging Types

### 2.1- Audit Logging

It is used to track changes to an entity in your project.

`IAuditLogger` should be injected into `DbContext`. `SaveChangesAsync` method from DbContext class can be overridden as follows

```csharp
class LibraryDbContext : Microsoft.EntityFrameworkCore.DbContext

{

    readonly IAuditLogger _auditLogger;

    LibraryDbContext(DbContextOptions options,IAuditLogger auditLogger) : base(options)

    {

    _auditLogger = auditLogger;

    }

    public DbSet<UserEntity> Users { get; set; }

    public DbSet<BookEntity> Books { get; set; }

    public DbSet<AuthorEntity> Authors { get; set; }


    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,

        CancellationToken cancellationToken = new ())

    {

        await _auditLogger.AddAsync(ChangeTracker, cancellationToken);
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

    }

}

```

It stores your data with the `AuditLoggerModel` model.

- <b>EntityId</b>: If the entity to be logged is derived from the IdenticalEntity class, the Id information of the Entity is obtained from here. With this Id, changes to the entity can be tracked more easily.
- <b>EntityJson</b>: It is the conversion of the Entity to json.
- <b>OperationDate</b>: Date of logging. Default value is now.
- <b>EntityOperationType</b>: It holds the change format of the Entity. (Added, Modified, Deleted)
- <b>OperatorId</b>: It holds the id information of the authorized user who performed the operation.
- <b>EntityTypeName</b>: It holds the class name of the Entity.

<img width="1264" alt="audit-log" src="https://github.com/user-attachments/assets/3039860b-ae88-4c2e-9401-731fd661a8af">



### 2.2- Exception Logging

It is used to track the exceptions thrown in your project.


A middleware pipeline is usually included in a project to make exceptions easier to manage. Let's examine how we provide this with `IExceptionLogger`
in the `GlobalExceptionHandlerMiddleware` in the sample code below.


```csharp

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
            var responseModel = new ApiBaseResponse();
            if (exception is CustomException customException)
            {
                responseModel.MessageCode = customException.MessageCode;
                responseModel.Message = ExceptionMessageHelper.GetMessage(responseModel.MessageCode);
            }

            await _exceptionLogger.AddAsync(context, exception, response.StatusCode, responseModel.MessageCode);
            var result = JsonSerializer.Serialize(responseModel);
            await response.WriteAsync(result);
        }
    }
}
```

It stores your data with the `ExceptionLoggerModel` model.

- <b>OperationDate</b>: Date of logging. Default value is now.
- <b>OperatorId</b>: It holds the id information of the authorized user who performed the operation.
- <b>RequestPath</b>: It holds the path information where the request was sent.
- <b>RequestRouteController</b>: It holds the controller information in the route.
- <b>RequestRouteAction</b>: It holds the action information in the route.
- <b>RequestMethod</b>: It holds the http method type information of the request.
- <b>RequestHost</b>: It holds the host information of the request.
- <b>RequestHeaderDicStr</b>: It holds the header information of the request.
- <b>RequestQuery</b>: It holds the QueryString information of the request.
- <b>RequestBodyJson</b>: It holds the body information of the request.
- <b>RequestCookieDicStr</b>: It holds the Cookie information of the request.
- <b>RequestFormDicStr</b>: It holds the Form information of the request.
- <b>StatusCode</b>: It holds the StatusCode information of the response.
- <b>MessageCode</b>: It holds the information of your customized message code that you returned in the API response.
- <b>Message</b>: It holds the information of your customized message that you returned in the API response.
- <b>InnerMessage</b>: It holds the InnerException.Message information of the caught Exception.
- <b>Source</b>: It holds the Source information of the caught Exception.
- <b>StackTrace</b>: It holds the StackTrace information of the caught Exception.
- <b>HResult</b>: It holds the HResult information of the caught Exception.

<img width="1264" alt="ex-log" src="https://github.com/user-attachments/assets/16eaf489-b15f-4de7-a5d0-5fc799378e13">


### 2.3- Action Logging

It is used to track requests to actions in the controller via an attribute.

```csharp

[ActionLogger]
[HttpPost]
public async Task<IActionResult> PostAsync([FromBody] BookEntity book)
{
    var bookEntity = await _libraryDbContext.Books.AddAsync(book);
    await _libraryDbContext.SaveChangesAsync();
    var response = new ApiBaseResponse<BookEntity>(bookEntity.Entity);
    return Ok(response);
}

```

It stores your data with `ActionLoggerModel` model.

- <b>OperationDate</b>: Date of logging. Default value is now.
- <b>OperatorId</b>: It holds the id information of the authorized user who performed the operation.
- <b>RequestPath</b>: It holds the path information where the request was sent.
- <b>RequestRouteController</b>: It holds the controller information in the route.
- <b>RequestRouteAction</b>: It holds the action information in the route.
- <b>RequestMethod</b>: It holds the http method type information of the request.
- <b>RequestHost</b>: It holds the host information of the request.
- <b>RequestHeaderDicStr</b>: It holds the header information of the request.
- <b>RequestQuery</b>: It holds the QueryString information of the request.
- <b>RequestBodyJson</b>: It holds the body information of the request.
- <b>RequestCookieDicStr</b>: It holds the Cookie information of the request.
- <b>RequestFormDicStr</b>: It holds the Form information of the request.
- <b>ActionId</b>: It is the Guid generated for each action log record. Common to Executing and Executed logs.
- <b>ResponseBodyJson</b>: Holds the Body information returned in the Action response.
- <b>ResponseStatusCode</b>: Holds the StatusCode information returned in the Action response.
- <b>ActionLoggerType</b>: Holds the information of which stage (Executing and Executed) the Action is in the log record.


<img width="1264" alt="action" src="https://github.com/user-attachments/assets/27c5484d-5ebe-468f-9d59-df5b2f8c212f">


## 3- Demonstration Project


- Clone the project
  
```shell
git clone https://github.com/mwlt68/MEG.git
```

- Database Migration
  
```shell
dotnet ef database update  --project "ElasticLogger.Api" 
```

- Launching ElasticSearch and Kibana containers
  
```shell
docker-compose up
```

- Running Demo Project 🚀

You can access the demo project [here](https://github.com/mwlt68/MEG/tree/logger/demo/MEG.Demo.ElasticLogger.Api)

## 4- Customization

### 4.1- Customizing the ElasticLogger

<b>GetOperatorId Method </b>: Used to set the OperatorId information in the logs. By default, it works with the User information in the HttpContext.

<b>HandleIndexResponse Method </b>: Returns an IndexResponse after a log is added to Elastic search. This method includes any post-logging function.

A class that implements the `IElasticLogger` class must be created.

```csharp
public class CustomElasticLogger(IHttpContextAccessor contextAccessor) : IElasticLogger
{
    public string? GetOperatorId()
    {
        return contextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier )
            ?.Value;
    }

    public Task HandleIndexResponseAsync(IndexResponse? indexResponse)
    {
        return Task.CompletedTask;
    }
}
```
### 4.2- Customizing the AuditLogger

A model derived from `AuditLoggerModel` should be created.

```csharp
public class CustomAuditLoggerModel :AuditLoggerModel
{
    public string CustomProperty{ get; set; }
}
```

A class implementing `IAuditLogger` should be created.

```csharp
public class CustomAuditLogger(
    IElasticClient elasticClient,
    IHttpContextAccessor httpContextAccessor,
    IOptions<ElasticLoggerSettings> elasticLoggerSettings,
    IElasticLogger elasticLogger)
    : AuditLogger<CustomAuditLoggerModel>(elasticClient, elasticLoggerSettings, elasticLogger)
{
    protected override CustomAuditLoggerModel GetAuditLoggerModel(object entity, string serializedEntity,
        string entityEntryState)
    {
        var model = base.GetAuditLoggerModel(entity, serializedEntity, entityEntryState);
        model.CustomProperty = "Custom Value";
        return model;
    }

    protected override JsonSerializerSettings GetJsonSerializerSettings() => new()
        { ContractResolver = new IgnoringVirtualPropertiesContractResolver() };
}
```
<img width="1264" alt="custom-audit" src="https://github.com/user-attachments/assets/4ff77024-aadf-44b0-8264-ab91f718734d">


### 4.3- Customizing the ExceptionLogger

A model derived from `ExceptionLoggerModel` must be created.

```csharp
public class CustomExceptionLoggerModel : ExceptionLoggerModel
{
    public string? CustomProperty { get; set; }
}
```
A class implementing `IExceptionLogger` must be created.

```csharp
public class CustomExceptionLogger(
    IElasticClient elasticClient,
    IOptions<ElasticLoggerSettings> elasticLoggerSettings,
    IElasticLogger elasticLogger)
    : ExceptionLogger<CustomExceptionLoggerModel>(elasticClient, elasticLoggerSettings, elasticLogger)
{
    protected override CustomExceptionLoggerModel GetElasticExceptionModel(Exception exception, int statusCode,
        string? messageCode)
    {
        var model = base.GetElasticExceptionModel(exception, statusCode, messageCode);
        model.CustomProperty = "Elastic Exception Model Value";
        return model;
    }
}
```
<img width="1277" alt="custom-ex" src="https://github.com/user-attachments/assets/9454a6f3-0f8c-4ef4-b70b-bfb9f2ff8d4e">


### 4.4- Customizing the ActionLogger

A model that derives from `ActionLoggerModel` must be created.

```csharp
public class CustomActionLoggerModel : ActionLoggerModel
{
    public string CustomProperty { get; set; }
}
```

A class that implements `IActionLogger` must be created.

```csharp
public class CustomActionLogger(
    IElasticClient elasticClient,
    IOptions<ElasticLoggerSettings> elasticLoggerSettings,
    IElasticLogger elasticLogger)
    : ActionLogger<CustomActionLoggerModel>(elasticClient, elasticLoggerSettings, elasticLogger)
{
    protected override CustomActionLoggerModel GetActionExecutedLoggerModel(ObjectResult? objectResult)
    {
        var model = base.GetActionExecutedLoggerModel(objectResult);
        model.CustomProperty = "Action Executed Logger Model Value";
        return model;
    }

    protected override CustomActionLoggerModel GetActionExecutingLoggerModel()
    {
        var model = base.GetActionExecutingLoggerModel();
        model.CustomProperty = "Action Executing Logger Model Value";
        return model;
    }
}
```
<img width="1277" alt="custom-action" src="https://github.com/user-attachments/assets/ca30d03a-646e-42ad-9866-9b72d3770cea">


### 4.5- Service injection should be made.

```csharp
builder.Services
    .AddElasticLogger<CustomElasticLogger, CustomAuditLogger, CustomAuditLoggerModel, CustomActionLogger,
        CustomActionLoggerModel, CustomExceptionLogger, CustomExceptionLoggerModel>(builder.Configuration);
```

















