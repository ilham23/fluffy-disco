using System.Collections.Generic;
using System.Linq;
using Data.Infrastructure;
using Model.Models;

namespace Data.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
    }

    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

    }
}