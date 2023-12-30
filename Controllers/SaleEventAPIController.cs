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
    public class SaleEventApiController : ControllerBase
    {
        private MiniMarketContext _context { get; }
        private IMapper _mapper { get; }

        private ILogger<SaleEventApiController> _logger;

        public SaleEventApiController(MiniMarketContext context, IMapper mapper, ILogger<SaleEventApiController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        //read
        [HttpGet]
        [Route("sales")]

        public async Task<ActionResult<IEnumerable<SalesEventDTO_Get>>> GetSaleEvents()
        {

            var sale_events = await _context.SaleEvents
                .Where(s => s.IsOpen == 1)
                .FirstOrDefaultAsync();

            var convertedSaleEvent = new SalesEventDTO_Get{
                Id = sale_events.Id,
                Name = sale_events.Name,
                description = "Săn sale đón lễ cùng GreenMart nào! Rất nhiều sản phẩm được khuyến mãi!",
                start_time = sale_events.StartTime.Value.ToString("MMM dd, yyyy HH:mm:ss"),

                end_time = sale_events.EndTime.Value.ToString("MMM dd, yyyy HH:mm:ss"),
        };


            return Ok(convertedSaleEvent);
        }


        [HttpGet("sales/sanpham")]
        public async Task<ActionResult<IEnumerable<ProductDto_GetSaleProduct>>> GetProductsInOpenEvent()
        {
            try
            {
                var openEvent = await _context.SaleEvents.FirstOrDefaultAsync(e => e.IsOpen == 1);

                if (openEvent == null)
                {
                    return NotFound("No open event found");
                }

                var saleItemsInOpenEvent = await _context.SaleItems
                .Include(item => item.Product).ThenInclude(p => p.Brand)
                .Include(item => item.Product).ThenInclude(c => c.Category)
                .Where(item => item.EventId == openEvent.Id)
                .ToListAsync();


                var productsInOpenEvent = saleItemsInOpenEvent
                .Select(item => new ProductDto_GetSaleProduct
                {
                    Id = item.Product.Id,
                    thumbnail = item.Product.Thumbnail,
                    Name = item.Product.Name,
                    reg_price = item.Product.RegPrice,
                    discount_percent = item.Product.DiscountPercent,
                    discount_price = item.Product.DiscountPrice,
                    description = item.Product.Description,
                    rating = item.Product.Rating,
                    Brand = _mapper.Map<BrandDTO_GetIdName>(item.Product.Brand),
                    category = item.Product.Category?.Name,
                    quantity = item.Quantity ,// Set the quantity here
                        remaining = 50,
                })
                    .ToList();

                return Ok(productsInOpenEvent);

            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error: {ex.Message}");

                return StatusCode(500, new
                {
                    status = "error",
                    message = "Failed to retrieve products in open event",
                    error = new
                    {
                        message = ex.Message,
                        // Include additional details about the error if needed
                    }
                });
            }
        }







    }
}
