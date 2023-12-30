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
    public class CategoryApiController : ControllerBase
    {
        private MiniMarketContext _context { get; }
        private IMapper _mapper { get; }

        private ILogger<CategoryApiController> _logger;

        public CategoryApiController(MiniMarketContext context, IMapper mapper, ILogger<CategoryApiController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        //read
        //[HttpGet]
        //[Route("danhmuc/{categoryGroupId}/danhmucnho")]
        //public async Task<ActionResult<IEnumerable<CategoryDTO_Get>>> GetCategories(int categoryGroupId)
        //{

        //    var categories = await _context.Categories
        //        .Include(p => p.CategoryGroup)
        //        .Where(p =>p.CategoryGroupId == categoryGroupId)
        //        .ToListAsync();

        //    var convertedCategories = _mapper.Map<IEnumerable<CategoryDTO_Get>>(categories);


        //    return Ok(convertedCategories);
        //}

        //read
        [HttpGet]
        [Route("danhmuc/sp/{category_group_id}/sanpham")]
        public async Task<ActionResult<IEnumerable<CategoryDTO_GetProducts>>> GetCategoriesProducts(int category_group_id)
        {

            var categories = await _context.Categories
                .Include(c => c.Products).ThenInclude(p => p.Brand)
                .Include(p => p.CategoryGroup)
                .Where(p => p.CategoryGroupId == category_group_id)
                .ToListAsync();

            var categoryDtos = categories.Select(c => new CategoryDTO_GetProducts
            {
                id = c.Id,
                name = c.Name,
                description = c.Description,
                thumbnail = c.Thumbnail,
                product_quantity = c.Products.Count,
                products = c.Products.Select(p => new ProductDto_Get
                {
                    Id = p.Id,
                    thumbnail = p.Thumbnail,
                    Name = p.Name,
                    reg_price = p.RegPrice,
                    discount_percent = p.DiscountPercent,
                    discount_price = p.DiscountPrice,
                    description = p.Description,
                    rating = p.Rating,
                    Brand = new BrandDTO_GetIdName
                    {
                        Id = p.Brand.Id,
                        Name = p.Brand.Name,
                        Thumbnail = p.Brand.Thumbnail,
                    },
                    category = p.Category.Name
                }).ToList()
            }).ToList();



            return Ok(categoryDtos);
        }


        [HttpGet("danhmucnho/{categoryId}/lienquan")]
        public async Task<ActionResult<IEnumerable<CategoryDTO_Get>>> GetRelatedProducts(int categoryId)
        {

            var category = await _context.Categories
                .Include(p => p.CategoryGroup)
                .FirstOrDefaultAsync(p => p.Id == categoryId);

            if (category == null)
            {
                return NotFound(); // Return 404 if the specified product is not found
            }

            var relatedCategories = await _context.Categories
            .Include(p => p.CategoryGroup)
            .Where(p => p.CategoryGroupId == category.CategoryGroupId && p.Id != categoryId)
            .ToListAsync();

            var convertedCategories = _mapper.Map<IEnumerable<CategoryDTO_Get>>(relatedCategories);


            return Ok(convertedCategories);
        }



    }
}
