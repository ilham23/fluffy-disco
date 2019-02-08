using AutoMapper;
using Model.Models;
using WebAPI.Models;
namespace WebAPI.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ProductCategory, ProductCategoryViewModel>();
                cfg.CreateMap<Product, ProductViewModel>();
                cfg.CreateMap<ProductCategoryViewModel, ProductCategory>();
                cfg.CreateMap<ProductViewModel, Product>();
            });
        }
    }
}