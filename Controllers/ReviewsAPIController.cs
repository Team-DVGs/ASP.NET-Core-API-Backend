using AutoMapper;
using Do_an_mon_hoc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System;

namespace Do_an_mon_hoc.Controllers
{
    [Route("api")]
    [ApiController]
    public class ReviewsApiController : ControllerBase
    {
        private MiniMarketContext _context { get; }
        private IMapper _mapper { get; }

        private ILogger<ReviewsApiController> _logger;

        public ReviewsApiController(MiniMarketContext context, IMapper mapper, ILogger<ReviewsApiController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        //Lay danh gia san pham
        [HttpGet("sanpham/{productId}/danhgia")]
        public async Task<ActionResult<IEnumerable<ReviewDTO_Get>>> GetReviewsForProduct(int productId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.ProductId == productId)
                .Include(c => c.User)
                .ToListAsync();

            if (reviews == null || !reviews.Any())
            {
                return NotFound(); // Return 404 if there are no reviews for the specified product
            }

            // Format the DateTime property before mapping

            // Map the entities to DTOs
            var reviewDtos = reviews.Select(r => new ReviewDTO_Get
            {
                // Map other properties...
                id = r.Id,
                rating = r.Rating,
                title = r.Title,
                comment = r.Comment,
                fullname = r.User.Fullname,
                created_at = r.CreatedAt.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            });

            return Ok(reviewDtos);
        }


        //them danh gia
        [HttpPost("sanpham/themdanhgia")]
        public async Task<ActionResult<ReviewDTO_Add>> PostReview( [FromBody] ReviewDTO_Add reviewDto)
        {
            //var product = await _context.Products.FindAsync(productId);

            //if (product == null)
            //{
            //    return NotFound(new
            //    {
            //        status = "error",
            //        message = "Product not found",
            //        error = new
            //        {
            //            // You can provide additional details about the error if needed
            //        }
            //    });
            //}
            try

            {
                
                // Map the DTO to the entity
                var review = new Review
                {

                    Rating = reviewDto.rating,
                    Title = reviewDto.title,
                    Comment = reviewDto.comment,
                    ProductId = reviewDto.productId,
                    UserId = reviewDto.userId,
                    CreatedAt = DateTime.Now,
                };
                
                //DateTimeOffset dateTimeOffset = DateTimeOffset.ParseExact(reviewDto.created_at, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                //review.CreatedAt = dateTimeOffset.DateTime;

                // Save the review to the database
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                var reviews = await _context.Reviews
                .Where(r => r.ProductId == reviewDto.productId)
                .Include(c => c.User)
                .ToListAsync();

                var reviewDtos = reviews.Select(r => new ReviewDTO_Get
                {
                    // Map other properties...
                    id = r.Id,
                    rating = r.Rating,
                    title = r.Title,
                    comment = r.Comment,
                    fullname = r.User.Fullname,
                    created_at = r.CreatedAt.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                });
                return Ok( reviewDtos
                );

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = "Failed to add review",
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
