using MEG.Demo.ElasticLogger.Api.DataAccess.Entities.Base;

namespace MEG.Demo.ElasticLogger.Api.DataAccess.Entities;

public class UserEntity : BaseEntity
{
    public string Username { get; set; }
    public string Password { get; set; }
}