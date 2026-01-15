using LibraryCoreWeb.Data;
using LibraryCoreWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryCoreWeb.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/members")]
    public class MembersController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public MembersController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddMember(Member member)
        {
            _context.Members.Add(member);
            await _context.SaveChangesAsync();
            return Ok(member);
        }
    }

}
