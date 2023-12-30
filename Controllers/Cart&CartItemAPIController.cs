using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Do_an_mon_hoc.Models;
using System.Linq;
using AutoMapper;
using Do_an_mon_hoc.Dto.Products;
using Do_an_mon_hoc.Models;


namespace Do_an_mon_hoc.Controllers
{
    [Route("api")]
    [ApiController]
    public class CartItemApiController : ControllerBase
    {
        private MiniMarketContext _context { get; }
        private IMapper _mapper { get; }

        private ILogger<CartItemApiController> _logger;

        public CartItemApiController(MiniMarketContext context, IMapper mapper, ILogger<CartItemApiController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        //read
        [HttpGet]
        [Route("giohang/{cartId}")]
        public async Task<ActionResult<CartDTO_Get>> GetCartItems(int cartId)
        {
            var user = await _context.Users
                .Include(u => u.Carts).ThenInclude(c => c.CartItems) // Include cart items
                    .ThenInclude(ci => ci.Product) // Include the Product entity for each CartItem
                .Where(u => u.Carts.Any(c => c.Id == cartId)) // Filter by cartId
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("User not found");
            }

            var cart = user.Carts.First(c => c.Id == cartId);

            double? totalOrderAmount = cart.CartItems.Sum(cartItem => cartItem.Total);
            double? saving = cart.CartItems.Sum(cartItem => (cartItem.Product.RegPrice - cartItem.Product.DiscountPrice));

            var convertedCartItems = new CartDTO_Get
            {
                Id = cartId,
                Quantity = cart.CartItems.Sum(item => item.Quantity),
                Total = totalOrderAmount,
                savings = saving,
                list = cart.CartItems.Select(item => new CartItemDTO_Get
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    Total = item.Total,
                    cartItemId = item.Id,
                    productId = item.Product.Id,
                    name = item.Product.Name,
                    reg_price = item.Product.RegPrice,
                    discount_price = item.Product.DiscountPrice,
                    Thumbnail = item.Product.Thumbnail,
                    cartid = cartId
                }).ToList()
            };

            return Ok(convertedCartItems);
        }







        [HttpPost]
        [Route("giohang/them")]
        public async Task<ActionResult<CartDTO_Get>> AddProductToCart([FromBody] CartItemDTO_Add cartItemDto)
        {
            try
            {
                // Fetch the associated product
                var product = await _context.Products.FindAsync(cartItemDto.productId);

                if (product == null)
                {
                    return NotFound("Product not found");
                }

                // Fetch the user's cart
                var userCart = await _context.Carts
                    .Include(c => c.CartItems).ThenInclude(p=> p.Product)
                    .Where(c => c.Id == cartItemDto.cartId)
                    .FirstOrDefaultAsync();

                if (userCart == null)
                {
                    return NotFound("Cart not found");
                }

                // Check if a cart item with the same product ID already exists
                var existingCartItem = userCart.CartItems
                    .FirstOrDefault(ci => ci.ProductId == cartItemDto.productId);
                
                if (existingCartItem != null)
                {
                    // Update the quantity of the existing cart item
                    existingCartItem.Quantity += cartItemDto.quantity;
                    existingCartItem.Total = existingCartItem.Quantity * product.DiscountPrice;
                }
                else
                {
                    // Create a new cart item
                    var newCartItem = new CartItem
                    {
                        Quantity = cartItemDto.quantity,
                        CartId = cartItemDto.cartId,
                        ProductId = cartItemDto.productId,
                        Total = cartItemDto.quantity * product.DiscountPrice,
                    };

                    // Add the new product to the context
                    await _context.CartItems.AddAsync(newCartItem);
                    userCart.CartItems.Add(newCartItem);
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                double? totalOrderAmount = userCart.CartItems.Sum(cartItem => cartItem.Total);
                double? saving = userCart.CartItems.Sum(cartItem => (cartItem.Product.RegPrice - cartItem.Product.DiscountPrice));
                

                // Retrieve the updated cart including the newly added item
                var updatedCart = await _context.Carts
                    .Include(c => c.CartItems).ThenInclude(p => p.Product)
                    .Where(c => c.Id == userCart.Id)
                    .FirstOrDefaultAsync();

                var cartDto = new CartDTO_Get
                {
                    Id = updatedCart.Id,
                    Quantity = updatedCart.Quantity,
                    Total = totalOrderAmount,
                    savings = saving,
                    list = updatedCart.CartItems
                        .Select(oi => new CartItemDTO_Get
                        {
                            Id = oi.Id,
                            cartid = oi.CartId,
                            Total = oi.Total,
                            cartItemId = oi.Id,
                            reg_price = oi.Product.RegPrice,
                            productId = oi.Product.Id,
                            name = oi.Product.Name,
                            Thumbnail = oi.Product.Thumbnail,
                            Quantity = oi.Quantity,
                            discount_price = oi.Product.DiscountPrice,
                        }).ToList()
                };

                return Ok(cartDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = "Failed to add product to cart",
                    error = new
                    {
                        message = ex.Message,
                        // You can include additional details about the error if needed
                    }
                });
            }
        }


        [HttpPut]
        [Route("giohang/capnhat/{id}")]
        public async Task<ActionResult<CartDTO_Get>> UpdateCartItems(int id, [FromBody] CartItemDTO_Update cartItemUpdateDto)
        {

            // Retrieve the existing product from the context
            var existingCartItem = await _context.CartItems
                .Include(p => p.Product)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingCartItem == null)
            {
                return NotFound("Product not found");
            }

            // Update the existing product with the data from the DTO
            _mapper.Map(cartItemUpdateDto, existingCartItem);

            existingCartItem.Total = existingCartItem.Quantity * existingCartItem.Product.DiscountPrice;
            // Save changes to the database
            await _context.SaveChangesAsync();

            // Map the updated product to the DTO and return it
            var updatedCart = await _context.Carts
                    .Include(c => c.CartItems).ThenInclude(p => p.Product)
                    .Where(c => c.Id == existingCartItem.CartId)
                    .FirstOrDefaultAsync();

            double? totalOrderAmount = updatedCart.CartItems.Sum(cartItem => cartItem.Total);
            double? saving = updatedCart.CartItems.Sum(cartItem => (cartItem.Product.RegPrice - cartItem.Product.DiscountPrice));

            var convertedCartItems = new CartDTO_Get
            {
                Id = id,
                Quantity = updatedCart.CartItems.Sum(item => item.Quantity),
                Total = totalOrderAmount,
                savings = saving,
                list = updatedCart.CartItems.Select(item => new CartItemDTO_Get
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    Total = item.Total,
                    cartItemId = item.Id,
                    productId = item.Product.Id,
                    name = item.Product.Name,
                    reg_price = item.Product.RegPrice,
                    discount_price = item.Product.DiscountPrice,
                    Thumbnail = item.Product.Thumbnail,
                    cartid = id
                }).ToList()
            };

            return Ok(convertedCartItems);


        }


        [HttpDelete]
        [Route("giohang/xoa/{cartItemId}")]
        public async Task<ActionResult<CartDTO_Get>> DeleteCartItem(int cartItemId)
        {
            try
            {
                // Find the cart item by id
                var cartItem = await _context.CartItems.FindAsync(cartItemId);

                

                if (cartItem == null)
                {
                    return NotFound(new
                    {
                        status = "error",
                        message = "Cart item not found",
                        error = new
                        {
                            // You can include additional details about the error if needed
                        }
                    });
                }
                int? id = cartItem.CartId;

                // Remove the cart item from the context
                _context.CartItems.Remove(cartItem);

                // Save changes to the database
                await _context.SaveChangesAsync();
                var updatedCart = await _context.Carts
                    .Include(c => c.CartItems).ThenInclude(p => p.Product)
                    .Where(c => c.Id == id)
                    .FirstOrDefaultAsync();

                double? totalOrderAmount = updatedCart.CartItems.Sum(cartItem => cartItem.Total);
                double? saving = updatedCart.CartItems.Sum(cartItem => (cartItem.Product.RegPrice - cartItem.Product.DiscountPrice));

                var convertedCartItems = new CartDTO_Get
                {
                    Id = updatedCart.Id,
                    Quantity = updatedCart.CartItems.Sum(item => item.Quantity),
                    Total = totalOrderAmount,
                    savings = saving,
                    list = updatedCart.CartItems.Select(item => new CartItemDTO_Get
                    {
                        Id = item.Id,
                        Quantity = item.Quantity,
                        Total = item.Total,
                        cartItemId = item.Id,
                        productId = item.Product.Id,
                        name = item.Product.Name,
                        reg_price = item.Product.RegPrice,
                        discount_price = item.Product.DiscountPrice,
                        Thumbnail = item.Product.Thumbnail,
                        cartid = updatedCart.Id,
                    }).ToList()
                };

                return Ok(convertedCartItems);


            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = "Failed to delete cart item",
                    error = new
                    {
                        message = ex.Message,
                        // You can include additional details about the error if needed
                    }
                });
            }
        }

        [HttpDelete]
        [Route("giohang/{cartId}/xoa")]
        public async Task<ActionResult<CartDTO_Get>> DeleteAllCartItems(int cartId)
        {
            try
            {
                // Find the cart by id
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.Id == cartId);

                if (cart == null)
                {
                    return NotFound(new
                    {
                        status = "error",
                        message = "Cart not found",
                        error = new
                        {
                            // You can include additional details about the error if needed
                        }
                    });
                }

                
                // Remove all cart items from the context
                _context.CartItems.RemoveRange(cart.CartItems);

                // Save changes to the database
                await _context.SaveChangesAsync();

                var updatedCart = await _context.Carts
                    .Include(c => c.CartItems).ThenInclude(p => p.Product)
                    .Where(c => c.Id == cartId)
                    .FirstOrDefaultAsync();
                double? totalOrderAmount = updatedCart.CartItems.Sum(cartItem => cartItem.Total);
                double? saving = updatedCart.CartItems.Sum(cartItem => (cartItem.Product.RegPrice - cartItem.Product.DiscountPrice));

                var convertedCartItems = new CartDTO_Get
                {
                    Id = updatedCart.Id,
                    Quantity = updatedCart.CartItems.Sum(item => item.Quantity),
                    Total = totalOrderAmount,
                    savings = saving,
                    list = updatedCart.CartItems.Select(item => new CartItemDTO_Get
                    {
                        Id = item.Id,
                        Quantity = item.Quantity,
                        Total = item.Total,
                        cartItemId = item.Id,
                        productId = item.Product.Id,
                        name = item.Product.Name,
                        reg_price = item.Product.RegPrice,
                        discount_price = item.Product.DiscountPrice,
                        Thumbnail = item.Product.Thumbnail,
                        cartid = updatedCart.Id
                    }).ToList()
                }; ;

                return Ok(convertedCartItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = "Failed to delete all cart items",
                    error = new
                    {
                        message = ex.Message,
                        // You can include additional details about the error if needed
                    }
                });
            }
        }




    }
}
