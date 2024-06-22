using MEG.ElasticLogger.Filters;
using Microsoft.AspNetCore.Mvc;

namespace MEG.ElasticLogger.Base.Attribute;

public class ActionLoggerAttribute() : TypeFilterAttribute(typeof(ActionLoggerFilter));