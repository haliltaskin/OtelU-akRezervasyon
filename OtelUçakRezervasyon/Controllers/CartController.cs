using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OtelUçakRezervasyon.Data;
using OtelUçakRezervasyon.DTOS.Cart;
using OtelUçakRezervasyon.Mappers;
using OtelUçakRezervasyon.Models;

namespace OtelUçakRezervasyon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Kullanıcı bilgisi alınamadı.");

            var cart = _context.Carts
                .Include(c => c.Items)
                .FirstOrDefault(c => c.UserId == userId); // int.Parse YOK!

            if (cart == null)
                return NotFound("Cart not found.");

            var cartDto = new CartDto
            {
                Id = cart.Id,
                Items = cart.Items.Select(i => new CartItemDto
                {
                    Id = i.Id,
                    HotelId = i.HotelId,
                    FlightId = i.FlightId,
                    Price = i.Price,
                    ItemType = i.ItemType
                }).ToList()
            };

            return Ok(cartDto);
        }

        [HttpPost("confirm")]
        public IActionResult ConfirmCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Kullanıcı bilgisi alınamadı.");

            var cart = _context.Carts
                .Include(c => c.Items)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null || !cart.Items.Any())
                return BadRequest("Sepet boş.");

            // ✅ Sipariş geçmişine ekle
            foreach (var item in cart.Items)
            {
                var confirmed = new ConfirmedReservation
                {
                    UserId = userId,
                    ItemType = item.ItemType,
                    HotelId = item.HotelId,
                    FlightId = item.FlightId,
                    Price = item.Price
                };
                _context.ConfirmedReservations.Add(confirmed);
            }

            // ✅ Sepeti temizle
            _context.CartItems.RemoveRange(cart.Items);
            _context.SaveChanges();

            return Ok("Rezervasyon onaylandı ve sipariş geçmişine eklendi.");
        }


        [HttpGet("orders")]
        public IActionResult GetPastOrders()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Kullanıcı bilgisi alınamadı.");

            var pastOrders = _context.ConfirmedReservations
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.ConfirmedAt)
                .ToList();

            return Ok(pastOrders);
        }




        // ✅ Sepetten Ürün Sil
        [HttpDelete("{cartId}/remove/{itemId}")]
        public IActionResult RemoveFromCart(int cartId, int itemId)
        {
            var cart = _context.Carts.Find(cartId);
            if (cart == null) return NotFound();

            var item = _context.CartItems.FirstOrDefault(i => i.Id == itemId && i.CartId == cartId);
            if (item == null) return NotFound();

            _context.CartItems.Remove(item);
            _context.SaveChanges();
            return NoContent();
        }

      
    }
}
