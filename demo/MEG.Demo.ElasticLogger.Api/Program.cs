
using MEG.Demo.ElasticLogger.Api.CustomLoggers;
using MEG.Demo.ElasticLogger.Api.DataAccess.DbContext;
using MEG.Demo.ElasticLogger.Api.Extensions;
using MEG.Demo.ElasticLogger.Api.Middleware;
using MEG.Demo.ElasticLogger.Api.Models.Custom;
using MEG.ElasticLogger.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

builder.Services.AddEntityFrameworkNpgsql().AddDbContext<LibraryDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("LibraryDb")));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCustomAuthentication();

builder.Services.AddCustomSwaggerGen();

builder.Services.AddElasticLogger(builder.Configuration);
/*
builder.Services
    .AddElasticLogger<CustomElasticLogger,
            CustomAuditLogger, CustomAuditLoggerModel,
            CustomActionLogger, CustomActionLoggerModel,
            CustomExceptionLogger, CustomExceptionLoggerModel>
        (builder.Configuration);
*/
var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();