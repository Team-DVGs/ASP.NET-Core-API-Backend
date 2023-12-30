using AutoMapper;
using Do_an_mon_hoc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System;
using Do_an_mon_hoc.Dto.CategoryGroup;

namespace Do_an_mon_hoc.Controllers
{
    [Route("api")]
    [ApiController]
    public class WishlistApiController : ControllerBase
    {
        private MiniMarketContext _context { get; }
        private IMapper _mapper { get; }

        private ILogger<WishlistApiController> _logger;

        public WishlistApiController(MiniMarketContext context, IMapper mapper, ILogger<WishlistApiController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("/api/sanpham/yeuthich")]
        public async Task<ActionResult<IEnumerable<WishlistDTO_Get>>> AddToWishlist([FromBody] WishlistDTO_Add wishlistDto)
        {
            try
            {
                // Check if the user and product exist
                var user = await _context.Users.FindAsync(wishlistDto.UserId);
                var product = await _context.Products.FindAsync(wishlistDto.ProductId);

                if (user == null || product == null)
                {
                    return NotFound("User or product not found");
                }

                // Check if the item is already in the wishlist
                if (_context.Wishlists.Any(w => w.UserId == wishlistDto.UserId && w.ProductId == wishlistDto.ProductId))
                {
                    return BadRequest("Sản phẩm đã có trong danh sách yêu thích");
                }

                // Map the DTO to the entity
                var wishListItem = new Wishlist
                {
                    UserId = wishlistDto.UserId,
                    ProductId = wishlistDto.ProductId
                    // Add other properties as needed
                };

                // Add the item to the wishlist
                await _context.Wishlists.AddAsync(wishListItem);
                await _context.SaveChangesAsync();

                // Retrieve the updated wishlist
                var updatedWishlist = await GetWishlistItems(wishlistDto.UserId);

                return Ok(updatedWishlist);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = "Failed to add to wishlist",
                    error = new
                    {
                        message = ex.Message,
                        // Additional details about the error if needed
                    }
                });
            }
        }


        private async Task<IEnumerable<WishlistDTO_Get>> GetWishlistItems(int? userId)
        {
            var wishlistItems = await _context.Wishlists
                .Where(w => w.UserId == userId)
                .Include(w => w.User)
                .Include(w => w.Product)
                .Select(w => new WishlistDTO_Get
                {
                    Id = w.Product.Id,
                    Thumbnail = w.Product.Thumbnail,
                    Name = w.Product.Name,
                    discount_price = w.Product.DiscountPrice
                    // Add other properties as needed
                })
                .ToListAsync();
            



            return wishlistItems; 
        }


        [HttpGet("/api/taikhoan/{userId}/yeuthich")]
        public async Task<ActionResult<IEnumerable<WishlistDTO_Get>>> GetWishlist(int userId)
        {
            try
            {
                // Retrieve the user's wishlist
                var wishlist = await _context.Wishlists
                    .Include(w => w.Product)
                    .Where(w => w.UserId == userId)
                    .ToListAsync();

                // Map the wishlist items to DTOs
                var wishlistDtos = wishlist.Select(w => new WishlistDTO_Get
                {
                    Id = w.Product.Id,
                    Thumbnail = w.Product.Thumbnail,
                    Name = w.Product.Name,
                    discount_price = w.Product.DiscountPrice,
                    // Add other properties as needed
                }).ToList();

                return Ok(wishlistDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = "Failed to get wishlist",
                    error = new
                    {
                        message = ex.Message,
                        // You can include additional details about the error if needed
                    }
                });
            }
        }

        [HttpDelete("/api/sanpham/yeuthich")]
        public async Task<ActionResult<IEnumerable<WishlistDTO_Get>>> RemoveFromWishlist([FromBody] WishlistDTO_Add wishlistDto)
        {
            try
            {
                // Check if the user and product exist
                var user = await _context.Users.FindAsync(wishlistDto.UserId);
                var product = await _context.Products.FindAsync(wishlistDto.ProductId);

                if (user == null || product == null)
                {
                    return NotFound("User or product not found");
                }

                // Find the wishlist item
                var wishlistItem = await _context.Wishlists
                    .Where(w => w.UserId == wishlistDto.UserId && w.ProductId == wishlistDto.ProductId)
                    .FirstOrDefaultAsync();

                if (wishlistItem == null)
                {
                    return BadRequest("Product is not in the wishlist");
                }

                // Remove the item from the wishlist
                _context.Wishlists.Remove(wishlistItem);
                await _context.SaveChangesAsync();

                // Retrieve the updated wishlist
                var updatedWishlist = await GetWishlistItems(wishlistDto.UserId);

                return Ok(updatedWishlist);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = "Failed to remove from wishlist",
                    error = new
                    {
                        message = ex.Message,
                        // Additional details about the error if needed
                    }
                });
            }
        }


    }
}
