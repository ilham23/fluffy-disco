using AutoMapper;
using Model.Models;
using WebApplication.Models;
namespace WebApplication.Mappings
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
                cfg.CreateMap<ProductViewModel,Product>();
            });
        }
    }
}