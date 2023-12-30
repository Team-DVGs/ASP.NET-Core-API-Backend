using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Do_an_mon_hoc.Models;
using System.Linq;
using AutoMapper;
using Do_an_mon_hoc.Dto.Products;
using Do_an_mon_hoc.Models;
using Do_an_mon_hoc.Dto.CategoryGroup;


namespace Do_an_mon_hoc.Controllers
{
    [Route("api")]
    [ApiController]
    public class CategoryGroupApiController : ControllerBase
    {
        private MiniMarketContext _context { get; }
        private IMapper _mapper { get; }

        private ILogger<CategoryGroupApiController> _logger;

        public CategoryGroupApiController(MiniMarketContext context, IMapper mapper, ILogger<CategoryGroupApiController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        //read
        [HttpGet]
        [Route("danhmuc")]

        public async Task<ActionResult<IEnumerable<CategoryGroupDTO_Get>>> GetCategoryGroups()
        {

            var category_group = await _context.CategoryGroups
                .ToListAsync();

            var convertedCategories = _mapper.Map<IEnumerable<CategoryGroupDTO_Get>>(category_group);


            return Ok(convertedCategories);
        }

        [HttpGet]
        [Route("danhmuc/{category_group_id}/ten")]

        public async Task<ActionResult<IEnumerable<CategoryGroupDTO_Get>>> GetCategoryGroup(int category_group_id)
        {

            var category_group = await _context.CategoryGroups
                .FirstOrDefaultAsync(p => p.Id == category_group_id);

            var convertedCategories = _mapper.Map<CategoryGroupDTO_Get>(category_group);


            return Ok(convertedCategories);
        }

        [HttpGet]
        [Route("danhmuc/{categoryGroupId}/danhmucnho")]
        public async Task<ActionResult<IEnumerable<object>>> GetCategories(int categoryGroupId)
        {

            var categoryGroup = await _context.CategoryGroups
            .Include(c => c.Categories)
            .Where(p => p.Id == categoryGroupId)
            .FirstOrDefaultAsync();

            if (categoryGroup == null)
            {
                return NotFound("Category group not found");
            }

            var convertedCategories = new
            {
                categoryGroupName = categoryGroup.Name,
                List = categoryGroup.Categories.Select(category => new CategoryDTO_Get
                {
                    id = category.Id,
                    name = category.Name,
                    thumbnail = category.Thumbnail
                }).ToList()
            };






            return Ok(convertedCategories);
        }

        [HttpGet("danhmuc/{categoryGroupId}/thuonghieu")]
        public async Task<ActionResult<IEnumerable<BrandDTO_GetIdName>>> GetDistinctBrandsByCategoryGroup(int categoryGroupId)
        {
            try
            {
                var distinctBrands = await _context.CategoryGroups
                    .Where(cg => cg.Id == categoryGroupId)
                    .SelectMany(cg => cg.Categories)
                    .SelectMany(c => c.Products)
                    .Select(p => p.Brand)
                    .Distinct()
                    .ToListAsync();

                var brandDtos = distinctBrands.Select(b => _mapper.Map<BrandDTO_GetIdName>(b)).ToList();

                return Ok(brandDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = "Failed to get distinct brands",
                    error = new
                    {
                        message = ex.Message,
                        // You can include additional details about the error if needed
                    }
                });
            }
        }

        [HttpGet("danhmucnho/{categoryId}/thuonghieu")]
        public async Task<ActionResult<IEnumerable<BrandDTO_GetIdName>>> GetDistinctBrandsByCategory(int categoryId)
        {
            try
            {
                var distinctBrands = await _context.Categories
                    .Where(cg => cg.Id == categoryId)
                    .SelectMany(c => c.Products)
                    .Select(p => p.Brand)
                    .Distinct()
                    .ToListAsync();

                var brandDtos = distinctBrands.Select(b => _mapper.Map<BrandDTO_GetIdName>(b)).ToList();

                return Ok(brandDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = "Failed to get distinct brands",
                    error = new
                    {
                        message = ex.Message,
                        // You can include additional details about the error if needed
                    }
                });
            }
        }

        [HttpGet("danhmuc/{id}/random")]
        public async Task<ActionResult<IEnumerable<CategoryGroupDTO_Get>>> GetRandomCategoryGroups(int id)
        {
            try
            {
                // Get the current category
                var currentCategoryGroup = await _context.CategoryGroups.FindAsync(id);

                if (currentCategoryGroup == null)
                {
                    return NotFound(new
                    {
                        status = "error",
                        message = "Category group not found",
                        error = new
                        {
                            // You can include additional details about the error if needed
                        }
                    });
                }

                // Get random categories excluding the current category
                var randomCategoryGroup = await _context.CategoryGroups
                    .Where(c => c.Id != id)
                    .OrderBy(r => Guid.NewGuid()) // Shuffle the categories randomly
                    .Take(4)
                    .ToListAsync();

                var categoryGroupDtos = randomCategoryGroup.Select(c => _mapper.Map<CategoryGroupDTO_Get>(c)).ToList();

                return Ok(categoryGroupDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = "Failed to get random categories",
                    error = new
                    {
                        message = ex.Message,
                        // You can include additional details about the error if needed
                    }
                });
            }
        }



        [HttpGet]
        [Route("sanpham/phobien")]
        public async Task<ActionResult<IEnumerable<CategoryGroupDTO_GetProducts>>> GetCategoriesGroupProducts()
        {
            try
            {
                // Fetch 5 CategoryGroups with their associated products
                var categoryGroups = await _context.CategoryGroups
                    .Include(cg => cg.Categories)
                        .ThenInclude(c => c.Products)
                            .ThenInclude(p => p.Brand)
                    .OrderBy(cg => Guid.NewGuid()) // Shuffle the CategoryGroups randomly
                    .Take(5)
                    .ToListAsync();

                var categoryGroupDtos = categoryGroups.Select(cg => new CategoryGroupDTO_GetProducts
                {
                    categoryID = cg.Id,
                    name = cg.Name,
                    thumbnail = cg.Thumbnail,
                    products = cg.Categories.SelectMany(c => c.Products)
                    .Take(12)
                    .Select(p => new ProductDto_Get
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

                return Ok(categoryGroupDtos);


            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = "Failed to get CategoryGroups with products",
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
