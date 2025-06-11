using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OtelUçakRezervasyon.Data;
using OtelUçakRezervasyon.DTOS.Comments;
using OtelUçakRezervasyon.Mappers;
using OtelUçakRezervasyon.Models;
using System.Security.Claims;

namespace OtelUçakRezervasyon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelCommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HotelCommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateComment([FromBody] CreateHotelCommentDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var comment = new HotelComment
            {
                AppUserId = int.Parse(userId),
                HotelId = dto.HotelId,
                CommentText = dto.CommentText,
                CreatedAt = DateTime.Now
            };

            _context.HotelComments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok("Yorum eklendi.");
        }

        [HttpGet("{hotelId}")]
        public async Task<IActionResult> GetComments(int hotelId)
        {
            var comments = await _context.HotelComments
                .Include(x => x.AppUser)
                .Where(x => x.HotelId == hotelId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            var result = comments.Select(c => c.ToHotelCommentDto());
            return Ok(result);
        }
    }
}
