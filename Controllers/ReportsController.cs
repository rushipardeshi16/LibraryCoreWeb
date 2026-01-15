using LibraryCoreWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryCoreWeb.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/reports")] 
    public class ReportsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public ReportsController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet("top-borrowed")]
        public async Task<IActionResult> TopBorrowed()
        {
            var result = await _context.BorrowRecords
                .GroupBy(br => br.BookId)
                .Select(g => new { BookId = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("overdue")]
        public async Task<IActionResult> Overdue()
        {
            var overdue = await _context.BorrowRecords
                .Where(br => !br.IsReturned &&
                       br.BorrowDate < DateTime.Now.AddDays(-14))
                .Join(_context.Members,
                    br => br.MemberId,
                    m => m.MemberId,
                    (br, m) => new
                    {
                        m.Name,
                        br.BookId,
                        br.BorrowDate
                    })
                .ToListAsync();

            return Ok(overdue);
        }
    }

}
