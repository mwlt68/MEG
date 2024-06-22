using MEG.Demo.ElasticLogger.Api.DataAccess.Entities.Base;

namespace MEG.Demo.ElasticLogger.Api.DataAccess.Entities;

public class AuthorEntity : BaseEntity
{

    public string NameSurname { get; set; }

    public virtual ICollection<BookEntity> Books { get; set; }
}