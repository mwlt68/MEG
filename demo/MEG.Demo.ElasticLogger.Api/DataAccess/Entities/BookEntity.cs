
using MEG.Demo.ElasticLogger.Api.DataAccess.Entities.Base;

namespace MEG.Demo.ElasticLogger.Api.DataAccess.Entities;

public class BookEntity:BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int PageCount { get; set; }
    public Guid AuthorId { get; set; }
    public DateTime PublicationDate { get; set; }
    public bool CanBeLoan { get; set; } 
    
    public virtual AuthorEntity Author { get; set; }
}