using LibraryCoreWeb.Data;
using LibraryCoreWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryCoreWeb.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class BorrowController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BorrowController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpPost("borrow")]
        public async Task<IActionResult> BorrowBook(int memberId, int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null || book.AvailableCopies == 0)
                return BadRequest("Book not available");

            book.AvailableCopies--;

            _context.BorrowRecords.Add(new BorrowRecord
            {
                MemberId = memberId,
                BookId = bookId
            });

            await _context.SaveChangesAsync();
            return Ok("Book borrowed successfully");
        }

        [HttpPost("return")]
        public async Task<IActionResult> ReturnBook(int borrowId)
        {
            var record = await _context.BorrowRecords.FindAsync(borrowId);
            if (record == null || record.IsReturned)
                return BadRequest("Invalid borrow record");

            record.IsReturned = true;
            record.ReturnDate = DateTime.Now;

            var book = await _context.Books.FindAsync(record.BookId);
            book.AvailableCopies++;

            await _context.SaveChangesAsync();
            return Ok("Book returned successfully");
        }
    }

}
