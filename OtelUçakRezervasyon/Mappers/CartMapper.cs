using OtelUçakRezervasyon.DTOS.Cart;
using OtelUçakRezervasyon.Models;

namespace OtelUçakRezervasyon.Mappers
{
    public static class CartMapper
    {
        public static CartDto ToCartDto(this Cart cart)
        {
            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CreatedAt = cart.CreatedAt,
                Items = cart.Items?.Select(i => new CartItemDto
                {
                    Id = i.Id,
                    HotelId = i.HotelId,
                    FlightId = i.FlightId,
                    Price = i.Price,
                    ItemType = i.ItemType
                }).ToList()
            };
        }

        public static CartItem ToCartItem(this AddToCartRequestDto dto)
        {
            return new CartItem
            {
                HotelId = dto.HotelId,
                FlightId = dto.FlightId,
                Price = dto.Price,
                ItemType = dto.ItemType
            };
        }
    }
}
