using System.Configuration;
using MEG.ElasticLogger.Base.Abstract;
using MEG.ElasticLogger.Base.Concrete;
using MEG.ElasticLogger.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nest;

namespace MEG.ElasticLogger.Extensions;

public static class IServiceCollectionExtension
{
    public static void AddElasticLogger<TElasticLogger, TAuditLogger, TAuditLoggerModel, TActionLogger,
        TActionLoggerModel, TExceptionLogger, TExceptionLoggerModel>(this IServiceCollection serviceCollection,
        IConfiguration configuration,
        ElasticClient? elasticClient = null)
        where TElasticLogger : class, IElasticLogger
        where TAuditLogger : AuditLogger<TAuditLoggerModel>
        where TAuditLoggerModel : AuditLoggerModel, new()
        where TActionLogger : ActionLogger<TActionLoggerModel>
        where TActionLoggerModel : ActionLoggerModel, new()
        where TExceptionLogger : ExceptionLogger<TExceptionLoggerModel>
        where TExceptionLoggerModel : ExceptionLoggerModel, new()
    {
        serviceCollection.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

        serviceCollection.Configure<ElasticLoggerSettings>(
            configuration.GetSection(nameof(ElasticLoggerSettings)));

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var elasticLoggerSettings = serviceProvider.GetRequiredService<IOptions<ElasticLoggerSettings>>().Value;

        if (!elasticLoggerSettings.IsActive)
            return;

        if (string.IsNullOrEmpty(elasticLoggerSettings.ElasticSearchUrl))
        {
            throw new SettingsPropertyNotFoundException(
                $"ElasticSearchUrl must set inside {nameof(ElasticLoggerSettings)} in  app settings!");
        }

        if (elasticClient is null)
        {
            var uri = new Uri(elasticLoggerSettings.ElasticSearchUrl);

            var settings = new ConnectionSettings(uri)
                .PrettyJson();

            if (!String.IsNullOrWhiteSpace(elasticLoggerSettings.DefaultIndex))
                settings = settings.DefaultIndex(elasticLoggerSettings.AuditLoggerIndex);

            elasticClient = new ElasticClient(settings);
        }

        serviceCollection.AddSingleton<IElasticClient>(elasticClient);

        if (elasticLoggerSettings.IsAuditLoggerActive)
            elasticClient.Indices
                .Create(elasticLoggerSettings.AuditLoggerIndex, i => i
                    .Map<TAuditLoggerModel>(x => x.AutoMap())
                );

        if (elasticLoggerSettings.IsExceptionLoggerActive)
            elasticClient.Indices.Create(elasticLoggerSettings.ExceptionLoggerIndex,
                i => i.Map<TExceptionLogger>(x => x.AutoMap()));

        if (elasticLoggerSettings.IsActionLoggerIndexActive)
            elasticClient.Indices
                .Create(elasticLoggerSettings.ActionLoggerIndex, i => i
                    .Map<TActionLoggerModel>(x => x.AutoMap())
                );

        serviceCollection.AddTransient<IElasticLogger, TElasticLogger>();
        serviceCollection.AddTransient<IAuditLogger, TAuditLogger>();
        serviceCollection.AddTransient<IExceptionLogger, TExceptionLogger>();
        serviceCollection.AddTransient<IActionLogger, TActionLogger>();
    }

    public static void AddElasticLogger(this IServiceCollection serviceCollection,
        IConfiguration configuration,
        ElasticClient? elasticClient = null)
    {
        serviceCollection
            .AddElasticLogger<Base.Concrete.ElasticLogger,
                    AuditLogger<AuditLoggerModel>, AuditLoggerModel,
                    ActionLogger<ActionLoggerModel>, ActionLoggerModel,
                    ExceptionLogger<ExceptionLoggerModel>,
                    ExceptionLoggerModel>
                (configuration, elasticClient);
    }

    public static void AddElasticLogger<TElasticLogger>(this IServiceCollection serviceCollection,
        IConfiguration configuration,
        ElasticClient? elasticClient = null)
        where TElasticLogger : class, IElasticLogger
    {
        serviceCollection
            .AddElasticLogger<TElasticLogger,
                    AuditLogger<AuditLoggerModel>, AuditLoggerModel,
                    ActionLogger<ActionLoggerModel>, ActionLoggerModel,
                    ExceptionLogger<ExceptionLoggerModel>,
                    ExceptionLoggerModel>
                (configuration, elasticClient);
    }

    public static void AddCustomElasticLoggerForAudit<TElasticLogger, TAuditLogger, TAuditLoggerModel>(
        this IServiceCollection serviceCollection,
        IConfiguration configuration,
        ElasticClient? elasticClient = null)
        where TElasticLogger : class, IElasticLogger
        where TAuditLogger : AuditLogger<TAuditLoggerModel>
        where TAuditLoggerModel : AuditLoggerModel, new()
    {
        serviceCollection
            .AddElasticLogger<TElasticLogger,
                    TAuditLogger, TAuditLoggerModel,
                    ActionLogger<ActionLoggerModel>, ActionLoggerModel,
                    ExceptionLogger<ExceptionLoggerModel>,
                    ExceptionLoggerModel>
                (configuration, elasticClient);
    }


    public static void AddCustomElasticLoggerForAction<TElasticLogger, TActionLogger,
        TActionLoggerModel>(this IServiceCollection serviceCollection,
        IConfiguration configuration,
        ElasticClient? elasticClient = null)
        where TElasticLogger : class, IElasticLogger
        where TActionLogger : ActionLogger<TActionLoggerModel>
        where TActionLoggerModel : ActionLoggerModel, new()
    {
        serviceCollection
            .AddElasticLogger<TElasticLogger,
                    AuditLogger<AuditLoggerModel>, AuditLoggerModel,
                    TActionLogger, TActionLoggerModel,
                    ExceptionLogger<ExceptionLoggerModel>, ExceptionLoggerModel>
                (configuration, elasticClient);
    }
}