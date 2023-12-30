using AutoMapper;
using Do_an_mon_hoc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Do_an_mon_hoc.Controllers
{
    [Route("api")]
    [ApiController]
    public class OrderAPIController : ControllerBase

    {

        private MiniMarketContext _context { get; }
        private IMapper _mapper { get; }

        private ILogger<OrderAPIController> _logger;

        public OrderAPIController(MiniMarketContext context, IMapper mapper, ILogger<OrderAPIController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpPost("/api/donhang/them")]
        public async Task<ActionResult<object>> CreateOrder([FromBody] OrderDTO_Add orderdto)
        {
            try
            {
                // Fetch the user based on the user ID
                var user = await _context.Users
                    .Include(u => u.Carts).ThenInclude(c => c.CartItems) // Include cart items
                    .Where(u => u.Id == orderdto.UserId)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound("User not found");
                }

                // Create a new order
                double? totalOrderAmount = user.Carts.First().CartItems.Sum(cartItem => cartItem.Total);

                // Create a new order
                var newOrder = new Order
                {
                    UserId = orderdto.UserId,
                    Address = orderdto.Address,
                    PaymentMethod = orderdto.payment_method,
                    Note = orderdto.Note,
                    Status = "Đang xử lý",
                    Total = totalOrderAmount, // Set the total order amount
                    CreatedAt = DateTime.Now,
                    // Add other properties as needed
                };

                // Add the order to the context
                await _context.Orders.AddAsync(newOrder);

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Move cart items to order items
                foreach (var cartItem in user.Carts.First().CartItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = newOrder.Id,
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,
                        Total = cartItem.Total,
                        Price = cartItem.Total / cartItem.Quantity,
                        // Add other properties as needed
                    };

                    _context.OrderItems.Add(orderItem);
                }

                // Clear cart items
                user.Carts.First().CartItems.Clear();

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Map the new order entity to a DTO
                var response = new
                {
                    id = newOrder.Id
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = "Failed to create order",
                    error = new
                    {
                        message = ex.Message,
                        // You can include additional details about the error if needed
                    }
                });
            }
        }


        [HttpGet("/api/taikhoan/{userId}/donhang")]
        public async Task<ActionResult<IEnumerable<OrderDTO_GetOrderHistory>>> GetUserOrderHistory(int userId)
        {
            try
            {
                // Fetch user orders based on the user ID
                var userOrders = await _context.Orders
                    .Include(p => p.OrderItems).ThenInclude(h => h.Product)
                    .Where(o => o.UserId == userId)
                    .OrderByDescending(o => o.CreatedAt) // Order by creation time descending
                    .ToListAsync();

                // Map the orders to the DTO
                var orderHistoryDTOs = userOrders.Select(order =>
                {
                    var firstOrderItem = order.OrderItems.FirstOrDefault();
                     
                    return new OrderDTO_GetOrderHistory
                    {
                        Id = order.Id,
                        Note = order.Note,
                        Address = order.Address,
                        Status = order.Status, // Assuming the Order entity has a Status property
                        Total = order.Total,   // Assuming the Order entity has a Total property
                        Date = order.CreatedAt.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),// Format date as specified
                        Thumbnail = firstOrderItem?.Product.Thumbnail ?? string.Empty // Assuming Product has a Thumbnail property
                    }; 
                }).ToList();

                return Ok(orderHistoryDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = "Failed to get user order history",
                    error = new
                    {
                        message = ex.Message,
                        // You can include additional details about the error if needed
                    }
                });
            }
        }

        [HttpGet("/api/donhang/{donhangId}")]
        public async Task<ActionResult<OrderDTO_Get>> GetOrderDetails(int donhangId)
        {
            try
            {
                // Fetch the order based on the order ID
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                     .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(o => o.Id == donhangId);

                if (order == null)
                {
                    return NotFound("Order not found");
                }

                // Map the order details to the DTO
                var orderDetailsDTO = new OrderDTO_Get
                {
                    Id = donhangId,
                    Address = order.Address,
                    Total = order.Total,
                    Note = order.Note,
                    Status = order.Status,
                    payment_method = order.PaymentMethod,
                    Date = order.CreatedAt.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"), // Adjust date format
                    list = order.OrderItems
                    .Select(oi => new OrderItemDTO_Get
                    {
                        OrderId = order.Id,
                        itemId = oi.Id,
                        productId = oi.Product.Id,
                        name = oi.Product.Name,
                        Thumbnail = oi.Product.Thumbnail,
                        Quantity = oi.Quantity,
                        Total = oi.Total
                    }).ToList()
                };

                return Ok(orderDetailsDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = "Failed to get order details",
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
