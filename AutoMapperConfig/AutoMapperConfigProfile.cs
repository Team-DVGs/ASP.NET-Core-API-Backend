using AutoMapper;
using Do_an_mon_hoc.Dto.CategoryGroup;
using Do_an_mon_hoc.Dto.Products;
using Do_an_mon_hoc.Models;

namespace Do_an_mon_hoc.AutoMapperConfig
{
    public class AutoMapperConfigProfile : Profile
    {
        public AutoMapperConfigProfile() {

            //Product
            CreateMap<Product, ProductDto_Get>()
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
            .ForMember(dest => dest.category, opt => opt.MapFrom(src=> src.Category.Name));
            CreateMap<Product, ProductDto_GetProductDetail>()
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.galleries, opt => opt.MapFrom(src => src.Galleries));
            CreateMap<ProductDto_Add, Product>();
            CreateMap<ProductDto_Update, Product>();

            CreateMap<Product, ProductDto_GetSaleProduct>()
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
            .ForMember(dest => dest.category, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.quantity, opt => opt.MapFrom(src => src.SaleItems.Sum(saleItem => saleItem.Quantity)));

            //Gallery
            CreateMap<Gallery, GalleryDTO_Get>();



            //Review
            CreateMap<Review, ReviewDTO_Get>()
            .ForMember(dest => dest.fullname, opt => opt.MapFrom(src => src.User.Fullname));
            CreateMap<ProductDto_Add, Product>();
            CreateMap<ProductDto_Update, Product>();

            //Category
            CreateMap<Category, CategoryDTO_Get>()
            .ForMember(dest => dest.category_group_id, opt => opt.MapFrom(src => src.CategoryGroup.Id))
            .ForMember(dest => dest.category_group_name, opt => opt.MapFrom(src => src.CategoryGroup.Name));
            CreateMap<Category, CategoryDTO_GetProducts>()
             .ForMember(dest => dest.products, opt => opt.MapFrom(src => src.Products));

            CreateMap<Category, CategoryDTO_GetIdName>();



            //CategoryGroup
            CreateMap<CategoryGroup, CategoryGroupDTO_Get>();
            CreateMap<CategoryGroup, CategoryGroupDTO_GetCategories>()
            .ForMember(dest => dest.list, opt => opt.MapFrom(src => src.Categories))
            .ForMember(dest => dest.categoryGroupName, opt => opt.MapFrom(src => src.Name));

            CreateMap<CategoryGroup, CategoryGroupDTO_GetProducts>()
            .ForMember(dest => dest.products, opt => opt.MapFrom(src =>
                src.Categories.SelectMany(c => c.Products).Take(12)));




            //Brand
            CreateMap<Brand, BrandDTO_GetIdName>();

            //CartItem
            CreateMap<CartItem, CartItemDTO_Get>()
            .ForMember(dest => dest.cartid, opt => opt.MapFrom(src => src.Cart.Id))
            .ForMember(dest => dest.productId, opt => opt.MapFrom(src => src.Product.Id))
            .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.reg_price, opt => opt.MapFrom(src => src.Product.RegPrice))
            .ForMember(dest => dest.discount_price, opt => opt.MapFrom(src => src.Product.DiscountPrice));

            CreateMap<CartItemDTO_Add, CartItem>();
            CreateMap<CartItemDTO_Update, CartItem>();


            //cart
            CreateMap<Cart, CartDTO_Get>()
           .ForMember(dest => dest.list, opt => opt.MapFrom(src => src.CartItems));


            //Sale
            CreateMap<SaleEvent, SalesEventDTO_Get>();
            CreateMap<SaleItem, SaleItemDTO_Get>();

            //Order
            CreateMap<OrderDTO_Add, Order>();
            CreateMap<Order, OrderDTO_GetOrderHistory>();

            CreateMap<Order, OrderDTO_Get>();

            //OrderItem
            CreateMap<OrderItem, OrderItemDTO_Get>()
            .ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src => src.Product.Thumbnail))
            .ForMember(dest => dest.productId, opt => opt.MapFrom(src => src.Product.Id))
            .ForMember(dest => dest.itemId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Product.Name));

            //Wishlist
            CreateMap<WishlistDTO_Add, Wishlist>();

            CreateMap<Wishlist, WishlistDTO_Get>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Product.Id))
           .ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src => src.Product.Thumbnail))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.discount_price, opt => opt.MapFrom(src => src.Product.DiscountPrice));


        }
    }
}
