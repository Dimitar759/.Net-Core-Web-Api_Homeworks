using Homework2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Homework2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpGet] //http://localhost:[port]/api/Book
        public ActionResult<List<Book>> GetAllBooks()
        {
            try
            {
                return Ok(StaticDb.Books);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("queryString")]
        public ActionResult<Book> GetByQueryString(int? index)
        {
            try
            {
                if (index == null)
                {
                    return BadRequest("Please enter an index");
                }

                if (index < 0)
                {
                    return BadRequest("Please enter a positive number");
                }

                if (index >= StaticDb.Books.Count)
                {
                    return NotFound($"There isnt a book on index {index}");
                }

                return Ok(StaticDb.Books[index.Value]);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("multipleQuery")]
        public ActionResult<List<Book>> FilterBooksByAuthorAndTitle(string? author, string? title)
        {
            try
            {
                if (string.IsNullOrEmpty(author) && string.IsNullOrEmpty(title))
                {
                    return BadRequest("You need to send at least one filter parameter");
                   
                }

                if (string.IsNullOrEmpty(author) && !string.IsNullOrEmpty(title))
                {

                    List<Book> filteredBooksByPriority = StaticDb.Books.Where(x => x.Title.ToLower().Contains(title.ToLower())).ToList();

                    return Ok(filteredBooksByPriority);
                }

                if (!string.IsNullOrEmpty(author) && string.IsNullOrEmpty(title))
                {
                    List<Book> filteredBooksByText = StaticDb.Books.Where(x => x.Author.ToLower().Contains(author.ToLower())).ToList();

                    return Ok(filteredBooksByText);
                }

                List<Book> filteredBooks = StaticDb.Books.Where(x => x.Author.ToLower().Contains(author.ToLower()) && x.Title.ToLower().Contains(title.ToLower())).ToList();
                return Ok(filteredBooks);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<Book> AddBookAndAuthor([FromBody] Book book)
        {
            try
            {
                if (book == null)
                {
                    return BadRequest("The book cannot be null");
                }

                if (string.IsNullOrEmpty(book.Author) || string.IsNullOrEmpty(book.Title))
                {
                    return BadRequest("You need to fill both of the parameters");
                }

                StaticDb.Books.Add(book);
                return StatusCode(StatusCodes.Status201Created, "The book was successfully created!");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

       

    }
}



