using LibraryCoreWeb.Data;
using LibraryCoreWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryCoreWeb.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(Book book)
        {
            if (book.AvailableCopies < 0)
                return BadRequest("Copies cannot be negative");

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return Ok(book);
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _context.Books
                .Select(b => new
                {
                    b.Title,
                    b.Author,
                    b.ISBN,
                    b.AvailableCopies,
                    IsAvailable = b.AvailableCopies > 0
                }).ToListAsync();

            return Ok(books);
        }
    }

}
