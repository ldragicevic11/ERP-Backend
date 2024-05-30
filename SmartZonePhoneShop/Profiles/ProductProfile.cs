using AutoMapper;
using SmartZonePhoneShop.DTO.ProductDTOs;
using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, CreateProductDTO>();
            CreateMap<CreateProductDTO, Product>();

            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();

            CreateMap<Product, UpdateProductDTO>();
            CreateMap<UpdateProductDTO, Product>();

        }
    }
}
