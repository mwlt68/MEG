# MEG.DependencyInjection

This library provides automatic service registration and property injection capabilities for .NET applications. With MEG.DependencyInjection, you can register all your services and perform property injection with a single line of code.

### Key features
<ul>
<li> Automatic service registration based on marker interfaces </li>
<li> Property injection to eliminate constructor injection hell</li>
<li> Support for all service lifetimes (Scoped, Singleton, Transient)</li>
<li> Keyed service registration support</li>
<li> Flexible configuration options </li> 
</ul>

## 1- Implementation

Include MEG.DependencyInjection library in your project

```csharp
dotnet add package MEG.DependencyInjection --version 1.0.0
```

Service registration

```csharp
builder.Services.AddServices();
```

## 2- Service Registration

### 2.1- Basic Service Registration

Create a service interface that inherits from one of the marker interfaces:

```csharp
// Scoped service example
public interface IUserService : IScopedService
{
    Task<User> GetUserAsync(int id);
    Task CreateUserAsync(User user);
}

public class UserService : IUserService
{
    public async Task<User> GetUserAsync(int id)
    {
        // Implementation here
        return new User();
    }

    public async Task CreateUserAsync(User user)
    {
        // Implementation here
    }
}
```

After calling <b> builder.Services.AddServices() </b> , UserService will be automatically registered as <b> scoped </b> service. 

### 2.2- All Service Lifetimes

You can use all service lifetimes with marker interfaces:
<ul>
<li> IScopedService - Registers as Scoped </li> 
<li> ISingletonService - Registers as Singleton </li> 
<li> ITransientService - Registers as Transient </li> 
<li> IKeyedScopedService - Registers as Keyed Scoped </li> 
<li> IKeyedSingletonService - Registers as Keyed Singleton </li> 
<li> IKeyedTransientService - Registers as Keyed Transient </li> 
</ul>

### 2.3- Keyed Services

Example for keyed transient service:

```csharp

public interface IPaymentService : IKeyedTransientService
{
    Task ProcessPaymentAsync(decimal amount);
}

public class CreditCardPaymentService : IPaymentService
{
    public static string ServiceKey => "CreditCard";

    public async Task ProcessPaymentAsync(decimal amount)
    {
        // Credit card payment implementation
    }
}

public class PayPalPaymentService : IPaymentService
{
    public static string ServiceKey => "PayPal";

    public async Task ProcessPaymentAsync(decimal amount)
    {
        // PayPal payment implementation
    }
}

// Usage
public class PaymentController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;

    public PaymentController(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [HttpPost("pay")]
    public async Task<IActionResult> Pay([FromQuery] string paymentMethod, [FromBody] decimal amount)
    {
        var paymentService = _serviceProvider.GetRequiredKeyedService<IPaymentService>(paymentMethod);
        await paymentService.ProcessPaymentAsync(amount);
        return Ok();
    }
}

```


### 2.4- Configuration Options

You can configure assembly scanning and ignored types: 

```csharp

var option = new AddServiceOption
{
    Assembly = typeof(UserService).Assembly, // Scan specific assembly
    IgnoredTypes = new[] { typeof(UserService) } // Ignore specific types
};

builder.Services.AddServices(option);
```


## 3- Property Injection


### 3.1- Constructor Injection Hell
Without property injection, services with many dependencies become difficult to manage:

```csharp
public class UserService : IUserService
{
private readonly IUserRepository _userRepository;
private readonly IEmailRepository _emailRepository;
private readonly ILogRepository _logRepository;
private readonly IFileRepository _fileRepository;
private readonly INotificationRepository _notificationRepository;
private readonly IAuditRepository _auditRepository;
private readonly IPermissionRepository _permissionRepository;
private readonly IHttpContextAccessor _httpContextAccessor;
private readonly ILogger<UserService> _logger;
private readonly IHostEnvironment _hostEnvironment;

    public UserService(
        IUserRepository userRepository,
        IEmailRepository emailRepository,
        ILogRepository logRepository,
        IFileRepository fileRepository,
        INotificationRepository notificationRepository,
        IAuditRepository auditRepository,
        IPermissionRepository permissionRepository,
        IHttpContextAccessor httpContextAccessor,
        ILogger<UserService> logger,
        IHostEnvironment hostEnvironment)
    {
        _userRepository = userRepository;
        _emailRepository = emailRepository;
        _logRepository = logRepository;
        _fileRepository = fileRepository;
        _notificationRepository = notificationRepository;
        _auditRepository = auditRepository;
        _permissionRepository = permissionRepository;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _hostEnvironment = hostEnvironment;
    }
}
```

### 3.2- Enable Property Injection

Enable property injection with configuration:

```csharp
builder.Services.AddServices(option: new AddServiceOption
{
    IsPropertyInjectionActive = true
});
```

### 3.3- Property Injection Usage
With property injection, the same service becomes much cleaner:

```csharp
public class UserService : IUserService
{
    public IUserRepository UserRepository { get; set; }
    public IEmailRepository EmailRepository { get; set; }
    public ILogRepository LogRepository { get; set; }
    public IFileRepository FileRepository { get; set; }
    public INotificationRepository NotificationRepository { get; set; }
    public IAuditRepository AuditRepository { get; set; }
    public IPermissionRepository PermissionRepository { get; set; }
    public IHttpContextAccessor HttpContextAccessor { get; set; }
    public ILogger<UserService> Logger { get; set; }
    public IHostEnvironment HostEnvironment { get; set; }
}
```

### 3.4- Base Service Only Injection

You can limit property injection to only IBaseService derived services:

```csharp
builder.Services.AddServices(option: new AddServiceOption
{
    IsPropertyInjectionActive = true,
    IsOnlyBaseServiceInject = true
});
```
With this setting, only properties that inherit from IBaseService (like repositories) will be injected automatically.  

### 3.5- Ignore Injection Attribute

You can exclude specific properties from injection using the <b>IgnoreInjectionAttribute</b>:

```csharp
public class UserService : IUserService
{
    public IUserRepository UserRepository { get; set; }

    [IgnoreInjection]
    public ILogger<UserService> Logger { get; set; } // This will not be injected
    
    public UserService(ILogger<UserService> logger)
    {
        Logger = logger; // Manual injection through constructor
    }
}
```

## 4- Demonstration Project

You can access the demo project (here)[https://github.com/mwlt68/MEG/tree/main/demo/MEG.Demo.DependencyInjection.Api]