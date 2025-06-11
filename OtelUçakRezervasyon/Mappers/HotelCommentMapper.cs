using System.Runtime.CompilerServices;
using OtelUçakRezervasyon.DTOS.Comments;
using OtelUçakRezervasyon.Models;

namespace OtelUçakRezervasyon.Mappers
{
    public static class HotelCommentMapper
    {
        public static HotelCommentDto ToHotelCommentDto(this HotelComment comment)
        {
            return new HotelCommentDto
            {
                Username = comment.AppUser?.UserName ?? "Unknown User",
                CommentText = comment.CommentText,
                CreatedAt = comment.CreatedAt
            };
        }
    }
}
