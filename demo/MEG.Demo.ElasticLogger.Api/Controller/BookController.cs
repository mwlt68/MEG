using MEG.Demo.ElasticLogger.Api.DataAccess.DbContext;
using MEG.Demo.ElasticLogger.Api.DataAccess.Entities;
using MEG.Demo.ElasticLogger.Api.Models;
using MEG.ElasticLogger.Base.Attribute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MEG.Demo.ElasticLogger.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [ActionLogger]
    public class BookController : ControllerBase
    {
        private readonly LibraryDbContext _libraryDbContext;

        public BookController(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var books = await _libraryDbContext.Books.ToListAsync();
            var response = new ApiBaseResponse<List<BookEntity>>(books);
            return Ok(response);
        }
        
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] BookEntity book)
        {
            var bookEntity = await _libraryDbContext.Books.AddAsync(book);
            await _libraryDbContext.SaveChangesAsync();
            var response = new ApiBaseResponse<BookEntity>(bookEntity.Entity);
            return Ok(response);
        }
        
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] BookEntity book)
        {
            var bookEntity =  _libraryDbContext.Books.Update(book);
            await _libraryDbContext.SaveChangesAsync();
            var response = new ApiBaseResponse<BookEntity>(bookEntity.Entity);
            return Ok(response);
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromQuery] Guid id)
        {
            var book = await _libraryDbContext.Books.FirstOrDefaultAsync(x=> x.Id == id);
            if (book is not null)
            {
                _libraryDbContext.Books.Remove(book);
                await _libraryDbContext.SaveChangesAsync();
                var response = new ApiBaseResponse(){IsSuccess = true};
                return Ok(response);
            }
            else return NotFound();
        }
    }
}
